using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CORE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCoordenadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "X",
                table: "Regioes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Y",
                table: "Regioes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "Regioes");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "Regioes");
        }
    }
}
