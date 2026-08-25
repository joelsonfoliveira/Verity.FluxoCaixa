using Verity.FluxoCaixa.Dominio.Entidades;

namespace Verity.FluxoCaixa.Aplicacao.Interfaces;

public interface ILancamentoRepositorio
{
    Task AdicionarAsync(Lancamento lancamento, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Lancamento>> ObterPorDataAsync(
        DateOnly data,
        CancellationToken cancellationToken = default
    );
}
