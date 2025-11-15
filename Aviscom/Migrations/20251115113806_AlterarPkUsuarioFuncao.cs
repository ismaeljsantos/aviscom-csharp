using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aviscom.Migrations
{
    /// <inheritdoc />
    public partial class AlterarPkUsuarioFuncao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosFuncoes_UsuariosFisicos_FkPessoaFisicaId",
                table: "UsuariosFuncoes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosFuncoes_UsuariosJuridicos_FkPessoaJuridicaId",
                table: "UsuariosFuncoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuariosFuncoes",
                table: "UsuariosFuncoes");

            migrationBuilder.AlterColumn<string>(
                name: "FkPessoaJuridicaId",
                table: "UsuariosFuncoes",
                type: "char(26)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(26)");

            migrationBuilder.AlterColumn<string>(
                name: "FkPessoaFisicaId",
                table: "UsuariosFuncoes",
                type: "char(26)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(26)");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "UsuariosFuncoes",
                type: "char(26)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "UsuariosFuncoes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "UsuariosFuncoes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuariosFuncoes",
                table: "UsuariosFuncoes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosFuncoes_FkPessoaFisicaId_FkPessoaJuridicaId_FkFuncaoId_FkSetorId",
                table: "UsuariosFuncoes",
                columns: new[] { "FkPessoaFisicaId", "FkPessoaJuridicaId", "FkFuncaoId", "FkSetorId" },
                unique: true,
                filter: "[FkPessoaFisicaId] IS NOT NULL AND [FkPessoaJuridicaId] IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_UsuarioFuncao_Usuario_Exclusivo",
                table: "UsuariosFuncoes",
                sql: "[FkPessoaFisicaId] IS NULL OR [FkPessoaJuridicaId] IS NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosFuncoes_UsuariosFisicos_FkPessoaFisicaId",
                table: "UsuariosFuncoes",
                column: "FkPessoaFisicaId",
                principalTable: "UsuariosFisicos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosFuncoes_UsuariosJuridicos_FkPessoaJuridicaId",
                table: "UsuariosFuncoes",
                column: "FkPessoaJuridicaId",
                principalTable: "UsuariosJuridicos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosFuncoes_UsuariosFisicos_FkPessoaFisicaId",
                table: "UsuariosFuncoes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuariosFuncoes_UsuariosJuridicos_FkPessoaJuridicaId",
                table: "UsuariosFuncoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuariosFuncoes",
                table: "UsuariosFuncoes");

            migrationBuilder.DropIndex(
                name: "IX_UsuariosFuncoes_FkPessoaFisicaId_FkPessoaJuridicaId_FkFuncaoId_FkSetorId",
                table: "UsuariosFuncoes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_UsuarioFuncao_Usuario_Exclusivo",
                table: "UsuariosFuncoes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsuariosFuncoes");

            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "UsuariosFuncoes");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "UsuariosFuncoes");

            migrationBuilder.AlterColumn<string>(
                name: "FkPessoaJuridicaId",
                table: "UsuariosFuncoes",
                type: "char(26)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "char(26)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FkPessoaFisicaId",
                table: "UsuariosFuncoes",
                type: "char(26)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "char(26)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuariosFuncoes",
                table: "UsuariosFuncoes",
                columns: new[] { "FkPessoaFisicaId", "FkPessoaJuridicaId", "FkFuncaoId", "FkSetorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosFuncoes_UsuariosFisicos_FkPessoaFisicaId",
                table: "UsuariosFuncoes",
                column: "FkPessoaFisicaId",
                principalTable: "UsuariosFisicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuariosFuncoes_UsuariosJuridicos_FkPessoaJuridicaId",
                table: "UsuariosFuncoes",
                column: "FkPessoaJuridicaId",
                principalTable: "UsuariosJuridicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
