using Verity.FluxoCaixa.Dominio.Enums;

namespace Verity.FluxoCaixa.Dominio.Entidades;

public class Lancamento
{
    public Guid Id { get; private set; }
    public DateOnly Data { get; private set; }
    public decimal Valor { get; private set; }
    public TipoLancamento Tipo { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public DateTime CriadoEm { get; private set; }

    public Lancamento(
        DateOnly data,
        decimal valor,
        TipoLancamento tipo,
        string descricao)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor deve ser maior que zero.", nameof(valor));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição é obrigatória.", nameof(descricao));

        Id = Guid.NewGuid();
        Data = data;
        Valor = valor;
        Tipo = tipo;
        Descricao = descricao;
        CriadoEm = DateTime.UtcNow;
    }
}
