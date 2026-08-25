# Verity.FluxoCaixa

API para controle de fluxo de caixa de um pequeno comércio: registro de lançamentos (créditos e débitos) e consulta do saldo diário consolidado.

Projeto desenvolvido como teste técnico para vaga de Desenvolvedor Backend.

## Sumário

- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Como rodar localmente](#como-rodar-localmente)
- [Testando a API](#testando-a-api)
- [Visualizando os dados](#visualizando-os-dados)
- [Rodando os testes automatizados](#rodando-os-testes-automatizados)
- [Decisões técnicas](#decisões-técnicas)
- [Requisitos não funcionais](#requisitos-não-funcionais)
- [Possíveis melhorias futuras](#possíveis-melhorias-futuras)

## Arquitetura

O projeto segue uma separação em camadas inspirada em Clean Architecture, com a dependência sempre apontando para dentro (em direção ao Domínio):

```
Verity.FluxoCaixa.Api            → controllers, middleware, injeção de dependência
Verity.FluxoCaixa.Aplicacao      → casos de uso, DTOs, interfaces (portas)
Verity.FluxoCaixa.Infraestrutura → EF Core, Sqlite, repositórios (implementações)
Verity.FluxoCaixa.Dominio        → entidades e regras de negócio (sem dependências externas)
```

- **Dominio** não depende de nenhum outro projeto — não sabe que existe banco de dados ou HTTP.
- **Aplicacao** define interfaces (ex: `ILancamentoRepositorio`) que descrevem o que ela precisa, sem saber quem implementa.
- **Infraestrutura** implementa essas interfaces com EF Core + Sqlite.
- **Api** conecta tudo via injeção de dependência (`Program.cs`) e expõe os endpoints HTTP.

Esse desacoplamento permite, por exemplo, trocar o Sqlite por outro banco alterando apenas a Infraestrutura e uma linha no `Program.cs`, sem tocar nas regras de negócio.

### Endpoints

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/lancamentos` | Registra um lançamento (crédito ou débito) |
| `GET` | `/api/lancamentos?data=yyyy-MM-dd` | Lista os lançamentos de uma data |
| `GET` | `/api/saldo-diario/{data}` | Retorna o saldo consolidado (créditos, débitos e saldo) de uma data |

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2026 (ou qualquer editor + .NET CLI)
- Opcional: [DB Browser for SQLite](https://sqlitebrowser.org/) para inspecionar o banco gerado
- Opcional: Postman ou `curl` para testar os endpoints manualmente

Não é necessário instalar nenhum banco de dados separadamente — o projeto usa Sqlite, que é embutido (um simples arquivo).

## Como rodar localmente

### Opção 1: Visual Studio 2026

1. Abra o arquivo `Verity.FluxoCaixa.slnx` na raiz do repositório.
2. Defina **Verity.FluxoCaixa.Api** como projeto de inicialização (botão direito → "Definir como Projeto de Inicialização").
3. Rode com `F5` (ou `Ctrl+F5` para rodar sem debugar).
4. O navegador deve abrir automaticamente no Swagger (`/swagger`), caso contrário basta acessar a URL manualmente adicionando ao final `/swagger`.

### Opção 2: linha de comando

```bash
dotnet run --project src/Verity.FluxoCaixa.Api
```

O terminal vai mostrar a URL da aplicação (por padrão `https://localhost:7030` e `http://localhost:5250`).

### O que acontece na primeira execução

Ao iniciar, a aplicação aplica automaticamente as migrations do Entity Framework Core (`Database.Migrate()`), criando o arquivo `fluxo-caixa.db` dentro de `src/Verity.FluxoCaixa.Api/` com a tabela `Lancamentos`. Não é necessário rodar nenhum comando de banco manualmente.

## Testando a API

### Via Swagger

Acesse `https://localhost:7030/swagger` (ajuste a porta se necessário) e use os botões "Try it out" de cada endpoint.

### Via curl / Postman

Registrar um lançamento de crédito:

```bash
curl --location 'https://localhost:7030/api/lancamentos' \
--header 'accept: text/plain' \
--header 'Content-Type: application/json' \
--data '{
  "data": "2026-08-25",
  "valor": 1,
  "tipo": 1,
  "descricao": "Venda"
}'
```

Registrar um lançamento de débito:

```bash
curl --location 'https://localhost:7030/api/lancamentos' \
--header 'accept: text/plain' \
--header 'Content-Type: application/json' \
--data '{
  "data": "2026-08-25",
  "valor": 1,
  "tipo": 2,
  "descricao": "Pagamento Fornecedor"
}'
```

Listar os lançamentos de uma data:

```bash
curl --location 'https://localhost:7030/api/lancamentos?data=2026-08-25' \
--header 'accept: text/plain'
```

Consultar o saldo consolidado do dia:

```bash
curl --location 'https://localhost:7030/api/saldo-diario/2026-08-25' \
--header 'accept: text/plain'
```

> `tipo: 1` = Crédito, `tipo: 2` = Débito (ver `TipoLancamento`).

## Visualizando os dados

O banco é um arquivo Sqlite comum, em `src/Verity.FluxoCaixa.Api/fluxo-caixa.db`. Para inspecionar:

1. Abra o **DB Browser for SQLite**.
2. **Open Database** → selecione o arquivo `fluxo-caixa.db`.
3. Aba **Browse Data** → tabela `Lancamentos`.

Como a aplicação mantém o arquivo aberto enquanto roda, use **File → Revert Changes** no DB Browser para atualizar a visualização após novos lançamentos.

## Rodando os testes automatizados

```bash
dotnet test
```

O projeto de testes (`tests/Verity.FluxoCaixa.Testes`) usa **xUnit** + **Moq**, com testes unitários cobrindo:

- **Domínio**: validação de `Lancamento` (valor deve ser maior que zero) e cálculo de `SaldoDiario` (soma de créditos/débitos, saldo podendo ficar negativo).
- **Aplicação**: `LancamentoService` (persistência e propagação de erros de validação) e `SaldoDiarioConsultaService` (cálculo do saldo a partir dos lançamentos).

## Decisões técnicas

- **Sqlite**: escolhido por não exigir instalação/configuração de um servidor de banco — quem for rodar o projeto só precisa do .NET SDK. Como o acesso a dados está isolado atrás de `ILancamentoRepositorio`, trocar por SQL Server/PostgreSQL no futuro exigiria mudar apenas a Infraestrutura e a linha `UseSqlite(...)` no `Program.cs`.
- **Saldo diário calculado de forma síncrona**: a cada consulta, o saldo é somado na hora a partir dos lançamentos daquele dia (não existe uma tabela de saldo pré-calculada).
- **Descrição do lançamento não é obrigatória**: apenas o valor é validado (deve ser maior que zero). A entidade `Lancamento` se protege sozinha no construtor, tornando impossível existir um lançamento com valor inválido em qualquer parte do sistema.
- **Middleware de tratamento de erros centralizado**: em vez de `try/catch` repetido em cada controller, um único middleware (`ExceptionHandlingMiddleware`) converte erros de validação de negócio em HTTP 400 e qualquer outro erro inesperado em HTTP 500, sem vazar detalhes internos.

## Requisitos não funcionais

O desafio pede que o serviço de lançamentos continue operante mesmo se a consolidação diária falhar, e que a consolidação tolere até 5% de perda em picos de 50 requisições/segundo.

Dado o prazo do desafio, optei por simplificar: **o saldo é sempre calculado de forma síncrona, direto dos lançamentos, sem um processo de consolidação separado.** Como não existe um "sistema de consolidação" independente, não há o que falhar separadamente do serviço de lançamentos — mas essa simplificação não demonstra em código o cenário de tolerância a picos com perda controlada descrito no desafio.

**Se o volume de leitura do saldo justificasse otimizar isso em produção**, a abordagem seria:

1. Lançamentos continuam sendo gravados de forma síncrona e imediata (fonte da verdade) — a escrita nunca depende da consolidação.
2. Após gravar, publicar um sinal assíncrono e não-bloqueante ("recalcular saldo do dia X") numa fila limitada, com uma política de descarte controlado quando estiver cheia (ex.: `BoundedChannelFullMode.DropWrite` em memória, ou o equivalente no broker escolhido) — é isso que atenderia à tolerância de até 5% de perda em picos, sem nunca bloquear a escrita do lançamento.
3. Um worker em background consome essa fila e recalcula o saldo do dia de forma idempotente (pode reprocessar quantas vezes for preciso, sempre chegando ao mesmo resultado), persistindo um saldo pré-calculado.
4. A leitura do saldo usa esse valor pré-calculado quando disponível; se ainda não existir (worker atrasado ou indisponível), calcula na hora a partir dos lançamentos como fallback — garantindo que a consulta nunca fique bloqueada esperando a consolidação.
5. Em produção, a fila seria um broker externo (RabbitMQ, Kafka ou Azure Service Bus) em vez de uma fila em memória, permitindo múltiplas instâncias da API e persistência das mensagens entre reinicializações.

## Possíveis melhorias futuras

- Introduzir a consolidação assíncrona (fila + worker) usando um broker real para suportar múltiplas instâncias.
- Testes de integração do repositório contra um banco Sqlite real (hoje a cobertura é só unitária, com o repositório validado manualmente).
- Paginação no endpoint de listagem de lançamentos.
- Empacotamento em container (Docker).
- Diagrama de arquitetura mais detalhado.
