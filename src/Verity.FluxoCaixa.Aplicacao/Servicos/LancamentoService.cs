using Verity.FluxoCaixa.Aplicacao.Dtos;
using Verity.FluxoCaixa.Aplicacao.Interfaces;
using Verity.FluxoCaixa.Dominio.Entidades;

namespace Verity.FluxoCaixa.Aplicacao.Servicos;

public class LancamentoService(ILancamentoRepositorio lancamentoRepositorio)
{
    public async Task<LancamentoResponse> RegistrarAsync(
        RegistrarLancamentoRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var lancamento = new Lancamento(
            request.Data,
            request.Valor,
            request.Tipo,
            request.Descricao
        );

        await lancamentoRepositorio.AdicionarAsync(lancamento, cancellationToken);

        return LancamentoResponse.ParaResponse(lancamento);
    }

    public async Task<IReadOnlyList<LancamentoResponse>> ObterPorDataAsync(
        DateOnly data,
        CancellationToken cancellationToken = default
    )
    {
        var lancamentos = await lancamentoRepositorio.ObterPorDataAsync(data, cancellationToken);
        return [.. lancamentos.Select(LancamentoResponse.ParaResponse)];
    }
}
