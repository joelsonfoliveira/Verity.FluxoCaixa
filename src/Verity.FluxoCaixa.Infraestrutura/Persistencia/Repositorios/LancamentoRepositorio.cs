using Microsoft.EntityFrameworkCore;
using Verity.FluxoCaixa.Aplicacao.Interfaces;
using Verity.FluxoCaixa.Dominio.Entidades;

namespace Verity.FluxoCaixa.Infraestrutura.Persistencia.Repositorios;

public class LancamentoRepositorio(FluxoCaixaDbContext contexto) : ILancamentoRepositorio
{
    public async Task AdicionarAsync(
        Lancamento lancamento,
        CancellationToken cancellationToken = default
    )
    {
        await contexto.Lancamentos.AddAsync(lancamento, cancellationToken);
        await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Lancamento>> ObterPorDataAsync(
        DateOnly data,
        CancellationToken cancellationToken = default
    )
    {
        return await contexto
            .Lancamentos.AsNoTracking()
            .Where(l => l.Data == data)
            .OrderBy(l => l.CriadoEm)
            .ToListAsync(cancellationToken);
    }
}
