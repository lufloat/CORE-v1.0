using CORE.Domain.Entities;
using CORE.DOMAIN.Entities;
using Microsoft.EntityFrameworkCore;

namespace CORE.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Civilizacao> Civilizacoes { get; set; }
    public DbSet<Regiao> Regioes { get; set; }
    public DbSet<EventoCivilizacional> EventosCivilizacionais { get; set; }
    public DbSet<Partida> Partidas { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Civilizacao>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(c => c.Era)
                .HasConversion<string>();

            entity.OwnsMany(c => c.HistoricoEventos);

        });

        modelBuilder.Entity<Regiao>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Nome)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(r => r.Terreno)
                .HasConversion<string>();

            entity.Property(r => r.CivilizacaoId)
                 .IsRequired();
        });

        modelBuilder.Entity<EventoCivilizacional>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Tipo)
                .HasConversion<string>();

            entity.Property(e => e.Titulo)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Descricao)
                .IsRequired()
                .HasMaxLength(500);
        });

        base.OnModelCreating(modelBuilder);
    }
}