using Microsoft.EntityFrameworkCore;
using Verity.FluxoCaixa.Dominio.Entidades;

namespace Verity.FluxoCaixa.Infraestrutura.Persistencia;

public class FluxoCaixaDbContext(DbContextOptions<FluxoCaixaDbContext> options) : DbContext(options)
{
    public DbSet<Lancamento> Lancamentos => Set<Lancamento>();
}