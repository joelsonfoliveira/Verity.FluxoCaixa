using Verity.FluxoCaixa.Dominio.Entidades;
using Verity.FluxoCaixa.Dominio.Enums;

namespace Verity.FluxoCaixa.Testes.EntidatesTests;

public class SaldoDiarioTests
{
    [Fact]
    public void DeveCalcularSaldoSomandoCreditosEDebitos()
    {
        var data = DateOnly.FromDateTime(DateTime.Today);
        var lancamentos = new List<Lancamento>
        {
            new(data, 200m, TipoLancamento.Credito, "Venda 1"),
            new(data, 50m, TipoLancamento.Debito, "Fornecedor"),
            new(data, 100m, TipoLancamento.Credito, "Venda 2"),
        };

        var saldoDiario = new SaldoDiario(data, lancamentos);

        Assert.Equal(300m, saldoDiario.TotalCreditos);
        Assert.Equal(50m, saldoDiario.TotalDebitos);
        Assert.Equal(250m, saldoDiario.Saldo);
    }

    [Fact]
    public void DeveRetornarSaldoZeroQuandoNaoHaLancamentos()
    {
        var saldoDiario = new SaldoDiario(DateOnly.FromDateTime(DateTime.Today), []);

        Assert.Equal(0m, saldoDiario.TotalCreditos);
        Assert.Equal(0m, saldoDiario.TotalDebitos);
        Assert.Equal(0m, saldoDiario.Saldo);
    }

    [Fact]
    public void DeveConsiderarApenasDebitosQuandoNaoHaCreditos()
    {
        var data = DateOnly.FromDateTime(DateTime.Today);
        var lancamentos = new List<Lancamento>
        {
            new(data, 80m, TipoLancamento.Debito, "Fornecedor"),
        };

        var saldoDiario = new SaldoDiario(data, lancamentos);

        Assert.Equal(0m, saldoDiario.TotalCreditos);
        Assert.Equal(80m, saldoDiario.TotalDebitos);
        Assert.Equal(-80m, saldoDiario.Saldo);
    }
}
