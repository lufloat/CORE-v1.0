using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CORE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaPartida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PartidaId",
                table: "Civilizacoes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventoHistorico",
                columns: table => new
                {
                    CivilizacaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Turno = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoHistorico", x => new { x.CivilizacaoId, x.Id });
                    table.ForeignKey(
                        name: "FK_EventoHistorico_Civilizacoes_CivilizacaoId",
                        column: x => x.CivilizacaoId,
                        principalTable: "Civilizacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Partidas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    TurnoAtual = table.Column<int>(type: "integer", nullable: false),
                    Encerrada = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Civilizacoes_PartidaId",
                table: "Civilizacoes",
                column: "PartidaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Civilizacoes_Partidas_PartidaId",
                table: "Civilizacoes",
                column: "PartidaId",
                principalTable: "Partidas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Civilizacoes_Partidas_PartidaId",
                table: "Civilizacoes");

            migrationBuilder.DropTable(
                name: "EventoHistorico");

            migrationBuilder.DropTable(
                name: "Partidas");

            migrationBuilder.DropIndex(
                name: "IX_Civilizacoes_PartidaId",
                table: "Civilizacoes");

            migrationBuilder.DropColumn(
                name: "PartidaId",
                table: "Civilizacoes");
        }
    }
}
