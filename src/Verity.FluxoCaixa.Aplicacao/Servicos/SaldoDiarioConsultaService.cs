using Verity.FluxoCaixa.Aplicacao.Dtos;
using Verity.FluxoCaixa.Aplicacao.Interfaces;
using Verity.FluxoCaixa.Dominio.Entidades;

namespace Verity.FluxoCaixa.Aplicacao.Servicos;

public class SaldoDiarioConsultaService(ILancamentoRepositorio lancamentoRepositorio)
{
    public async Task<SaldoDiarioResponse> ObterAsync(
        DateOnly data,
        CancellationToken cancellationToken = default
    )
    {
        var lancamentosDoDia = await lancamentoRepositorio.ObterPorDataAsync(
            data,
            cancellationToken
        );
        var saldoDiario = new SaldoDiario(data, lancamentosDoDia);

        return SaldoDiarioResponse.ParaResponse(saldoDiario);
    }
}
