using Moq;
using Verity.FluxoCaixa.Aplicacao.Dtos;
using Verity.FluxoCaixa.Aplicacao.Interfaces;
using Verity.FluxoCaixa.Aplicacao.Servicos;
using Verity.FluxoCaixa.Dominio.Entidades;
using Verity.FluxoCaixa.Dominio.Enums;

namespace Verity.FluxoCaixa.Testes.Aplicacao.Servicos;

public class LancamentoServiceTests
{
    private readonly Mock<ILancamentoRepositorio> repositorioMock = new();

    [Fact]
    public async Task DevePersistirLancamentoValidoERetornarResponse()
    {
        var service = new LancamentoService(repositorioMock.Object);
        var request = new RegistrarLancamentoRequest(
            DateOnly.FromDateTime(DateTime.Today),
            100m,
            TipoLancamento.Credito,
            "Venda"
        );

        var response = await service.RegistrarAsync(request);

        repositorioMock.Verify(
            r => r.AdicionarAsync(It.IsAny<Lancamento>(), It.IsAny<CancellationToken>()),
            Times.Once
        );

        Assert.Equal(request.Valor, response.Valor);
        Assert.Equal(request.Tipo, response.Tipo);
        Assert.Equal(request.Descricao, response.Descricao);
    }

    [Fact]
    public async Task NaoDevePersistirQuandoLancamentoEInvalido()
    {
        var service = new LancamentoService(repositorioMock.Object);
        var request = new RegistrarLancamentoRequest(
            DateOnly.FromDateTime(DateTime.Today),
            0m,
            TipoLancamento.Credito,
            "Venda"
        );

        await Assert.ThrowsAsync<ArgumentException>(() => service.RegistrarAsync(request));

        repositorioMock.Verify(
            r => r.AdicionarAsync(It.IsAny<Lancamento>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async Task DeveRetornarLancamentosDaDataInformada()
    {
        var data = DateOnly.FromDateTime(DateTime.Today);
        var lancamentos = new List<Lancamento> { new(data, 100m, TipoLancamento.Credito, "Venda") };
        repositorioMock
            .Setup(r => r.ObterPorDataAsync(data, It.IsAny<CancellationToken>()))
            .ReturnsAsync(lancamentos);

        var service = new LancamentoService(repositorioMock.Object);

        var resultado = await service.ObterPorDataAsync(data);

        Assert.Single(resultado);
        Assert.Equal(100m, resultado[0].Valor);
    }

    [Fact]
    public async Task DeveRetornarListaVaziaQuandoNaoHaLancamentosNaData()
    {
        var data = DateOnly.FromDateTime(DateTime.Today);
        repositorioMock
            .Setup(r => r.ObterPorDataAsync(data, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var service = new LancamentoService(repositorioMock.Object);

        var resultado = await service.ObterPorDataAsync(data);

        Assert.Empty(resultado);
    }
}
