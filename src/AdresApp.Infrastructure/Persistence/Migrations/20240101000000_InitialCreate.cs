using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdresApp.Infrastructure.Persistence.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CarguesArchivo",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                IdArchivo = table.Column<int>(type: "int", nullable: false),
                IdPrestador = table.Column<int>(type: "int", nullable: false),
                NombreArchivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                RespuestaCargue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                RespuestaEstado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                MensajeError = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_CarguesArchivo", x => x.Id));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "CarguesArchivo");
    }
}
