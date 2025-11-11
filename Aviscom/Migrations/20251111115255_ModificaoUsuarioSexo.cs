using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aviscom.Migrations
{
    /// <inheritdoc />
    public partial class ModificaoUsuarioSexo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeSocial",
                table: "UsuariosFisicos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "UsuariosFisicos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeSocial",
                table: "UsuariosFisicos");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "UsuariosFisicos");
        }
    }
}
