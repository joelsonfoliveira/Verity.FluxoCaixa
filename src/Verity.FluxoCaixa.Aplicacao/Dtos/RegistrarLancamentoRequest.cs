using Verity.FluxoCaixa.Dominio.Enums;

namespace Verity.FluxoCaixa.Aplicacao.Dtos;

public record RegistrarLancamentoRequest(
    DateOnly Data,
    decimal Valor,
    TipoLancamento Tipo,
    string Descricao
);
