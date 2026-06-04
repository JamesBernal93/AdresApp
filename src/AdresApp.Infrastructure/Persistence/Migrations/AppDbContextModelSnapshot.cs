using AdresApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace AdresApp.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.8")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("AdresApp.Domain.Entities.CargueArchivo", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));
            b.Property<int>("IdArchivo").HasColumnType("int");
            b.Property<int>("IdPrestador").HasColumnType("int");
            b.Property<string>("NombreArchivo").IsRequired().HasMaxLength(500).HasColumnType("nvarchar(500)");
            b.Property<string>("Estado").IsRequired().HasMaxLength(50).HasColumnType("nvarchar(50)");
            b.Property<DateTime>("FechaCreacion").HasColumnType("datetime2");
            b.Property<DateTime?>("FechaActualizacion").HasColumnType("datetime2");
            b.Property<string>("RespuestaCargue").HasColumnType("nvarchar(max)");
            b.Property<string>("RespuestaEstado").HasColumnType("nvarchar(max)");
            b.Property<string>("MensajeError").HasMaxLength(2000).HasColumnType("nvarchar(2000)");
            b.HasKey("Id");
            b.ToTable("CarguesArchivo");
        });
#pragma warning restore 612, 618
    }
}
