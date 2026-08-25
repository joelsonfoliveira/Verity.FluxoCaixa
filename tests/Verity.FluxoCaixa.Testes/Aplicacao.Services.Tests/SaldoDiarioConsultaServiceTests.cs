using Moq;
using Verity.FluxoCaixa.Aplicacao.Interfaces;
using Verity.FluxoCaixa.Aplicacao.Servicos;
using Verity.FluxoCaixa.Dominio.Entidades;
using Verity.FluxoCaixa.Dominio.Enums;

namespace Verity.FluxoCaixa.Testes.Aplicacao.Servicos;

public class SaldoDiarioConsultaServiceTests
{
    [Fact]
    public async Task DeveCalcularSaldoAPartirDosLancamentosDoDia()
    {
        var data = DateOnly.FromDateTime(DateTime.Today);
        var lancamentos = new List<Lancamento>
        {
            new(data, 300m, TipoLancamento.Credito, "Venda 1"),
            new(data, 120m, TipoLancamento.Debito, "Fornecedor 1"),
            new(data, 120m, TipoLancamento.Debito, "Fornecedor 2"),
            new(data, 300m, TipoLancamento.Credito, "Venda 2"),
        };

        var repositorioMock = new Mock<ILancamentoRepositorio>();
        repositorioMock
            .Setup(r => r.ObterPorDataAsync(data, It.IsAny<CancellationToken>()))
            .ReturnsAsync(lancamentos);

        var service = new SaldoDiarioConsultaService(repositorioMock.Object);

        var response = await service.ObterAsync(data);

        Assert.Equal(600m, response.TotalCreditos);
        Assert.Equal(240m, response.TotalDebitos);
        Assert.Equal(360m, response.Saldo);
    }

    [Fact]
    public async Task DeveRetornarSaldoZeroQuandoNaoHaLancamentosNoDia()
    {
        var data = DateOnly.FromDateTime(DateTime.Today);
        var repositorioMock = new Mock<ILancamentoRepositorio>();
        repositorioMock
            .Setup(r => r.ObterPorDataAsync(data, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = new SaldoDiarioConsultaService(repositorioMock.Object);

        var response = await service.ObterAsync(data);

        Assert.Equal(0m, response.Saldo);
    }
}
