using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aviscom.Migrations
{
    /// <inheritdoc />
    public partial class ImplementarSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "UsuariosJuridicos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "UsuariosFuncoes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "UsuariosFisicos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "Setores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "Instituicoes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "Funcoes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "ExperienciasProfissionais",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "Escolaridades",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "Endereco",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "Empresas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAtivo",
                table: "Contatos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "UsuariosJuridicos");

            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "UsuariosFuncoes");

            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "UsuariosFisicos");

            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "Setores");

            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "Instituicoes");

            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "Funcoes");

            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "ExperienciasProfissionais");

            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "Escolaridades");

            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "Endereco");

            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "IsAtivo",
                table: "Contatos");
        }
    }
}
