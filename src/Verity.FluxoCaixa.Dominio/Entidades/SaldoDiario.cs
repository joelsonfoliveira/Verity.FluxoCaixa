using Verity.FluxoCaixa.Dominio.Enums;

namespace Verity.FluxoCaixa.Dominio.Entidades;

public class SaldoDiario
{
    public DateOnly Data { get; }
    public decimal TotalCreditos { get; }
    public decimal TotalDebitos { get; }
    public decimal Saldo => TotalCreditos - TotalDebitos;

    public SaldoDiario(DateOnly data, IEnumerable<Lancamento> lancamentosDoDia)
    {
        Data = data;

        foreach (var lancamento in lancamentosDoDia)
        {
            if (lancamento.Tipo == TipoLancamento.Credito)
                TotalCreditos += lancamento.Valor;
            else
                TotalDebitos += lancamento.Valor;
        }
    }
}
