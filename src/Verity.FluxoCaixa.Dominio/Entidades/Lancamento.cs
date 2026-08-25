using Verity.FluxoCaixa.Dominio.Enums;

namespace Verity.FluxoCaixa.Dominio.Entidades;

public class Lancamento
{
    public Guid Id { get; set; }
    public DateOnly Data { get; set; }
    public decimal Valor { get; set; }
    public TipoLancamento Tipo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }

    private Lancamento() { }

    public Lancamento(DateOnly data, decimal valor, TipoLancamento tipo, string descricao)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor deve ser maior que zero.", nameof(valor));

        Id = Guid.NewGuid();
        Data = data;
        Valor = valor;
        Tipo = tipo;
        Descricao = descricao;
        CriadoEm = DateTime.UtcNow;
    }
}
