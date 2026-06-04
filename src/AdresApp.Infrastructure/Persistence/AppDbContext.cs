using AdresApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdresApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CargueArchivo> CarguesArchivo => Set<CargueArchivo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CargueArchivo>(entity =>
        {
            entity.ToTable("CarguesArchivo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IdArchivo).IsRequired();
            entity.Property(e => e.IdPrestador).IsRequired();
            entity.Property(e => e.NombreArchivo).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).IsRequired();
            entity.Property(e => e.RespuestaCargue).HasColumnType("nvarchar(max)");
            entity.Property(e => e.RespuestaEstado).HasColumnType("nvarchar(max)");
            entity.Property(e => e.MensajeError).HasMaxLength(2000);
        });
        base.OnModelCreating(modelBuilder);
    }
}
