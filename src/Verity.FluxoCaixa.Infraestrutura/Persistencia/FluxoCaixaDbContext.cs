using Microsoft.EntityFrameworkCore;
using Verity.FluxoCaixa.Dominio.Entidades;

namespace Verity.FluxoCaixa.Infraestrutura.Persistencia;

public class FluxoCaixaDbContext(DbContextOptions<FluxoCaixaDbContext> options) : DbContext(options)
{
    public DbSet<Lancamento> Lancamentos => Set<Lancamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lancamento>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Descricao).HasMaxLength(200);
            entity.HasIndex(l => l.Data);
        });
    }
}
