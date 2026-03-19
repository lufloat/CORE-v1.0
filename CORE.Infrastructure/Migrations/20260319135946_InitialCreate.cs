using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CORE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Civilizacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Turno = table.Column<int>(type: "integer", nullable: false),
                    Populacao = table.Column<int>(type: "integer", nullable: false),
                    Comida = table.Column<int>(type: "integer", nullable: false),
                    Madeira = table.Column<int>(type: "integer", nullable: false),
                    Pedra = table.Column<int>(type: "integer", nullable: false),
                    Moral = table.Column<int>(type: "integer", nullable: false),
                    Tecnologia = table.Column<int>(type: "integer", nullable: false),
                    PoderMilitar = table.Column<int>(type: "integer", nullable: false),
                    Territorios = table.Column<int>(type: "integer", nullable: false),
                    Era = table.Column<string>(type: "text", nullable: false),
                    UltimoEvento = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Civilizacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventosCivilizacionais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ImpactoPopulacao = table.Column<int>(type: "integer", nullable: false),
                    ImpactoComida = table.Column<int>(type: "integer", nullable: false),
                    ImpactoMadeira = table.Column<int>(type: "integer", nullable: false),
                    ImpactoTecnologia = table.Column<int>(type: "integer", nullable: false),
                    ImpactoMoral = table.Column<int>(type: "integer", nullable: false),
                    ImpactoPoderMilitar = table.Column<int>(type: "integer", nullable: false),
                    ImpactoPedra = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosCivilizacionais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regioes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Terreno = table.Column<string>(type: "text", nullable: false),
                    Controlada = table.Column<bool>(type: "boolean", nullable: false),
                    ProducaoComida = table.Column<int>(type: "integer", nullable: false),
                    ProducaoMadeira = table.Column<int>(type: "integer", nullable: false),
                    ProducaoPedra = table.Column<int>(type: "integer", nullable: false),
                    NivelDesenvolvimento = table.Column<int>(type: "integer", nullable: false),
                    CivilizacaoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regioes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Civilizacoes");

            migrationBuilder.DropTable(
                name: "EventosCivilizacionais");

            migrationBuilder.DropTable(
                name: "Regioes");
        }
    }
}
