using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aviscom.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(26)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNPJ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funcoes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(26)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instituicoes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(26)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNPJ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instituicoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setores",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(26)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosFisicos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(26)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CpfCriptografado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CpfHash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NomeMae = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomePai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenhaHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataUltimoLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosFisicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Escolaridades",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(26)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeCurso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnoInicio = table.Column<int>(type: "int", nullable: true),
                    AnoConclusao = table.Column<int>(type: "int", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    FkUsuarioId = table.Column<string>(type: "char(26)", nullable: false),
                    FkInstituicaoId = table.Column<string>(type: "char(26)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escolaridades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Escolaridades_Instituicoes_FkInstituicaoId",
                        column: x => x.FkInstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Escolaridades_UsuariosFisicos_FkUsuarioId",
                        column: x => x.FkUsuarioId,
                        principalTable: "UsuariosFisicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienciasProfissionais",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(26)", nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnoEntrada = table.Column<int>(type: "int", nullable: true),
                    AnoSaida = table.Column<int>(type: "int", nullable: true),
                    DescricaoAtividades = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FkUsuarioId = table.Column<string>(type: "char(26)", nullable: false),
                    FkEmpresaId = table.Column<string>(type: "char(26)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienciasProfissionais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienciasProfissionais_Empresas_FkEmpresaId",
                        column: x => x.FkEmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperienciasProfissionais_UsuariosFisicos_FkUsuarioId",
                        column: x => x.FkUsuarioId,
                        principalTable: "UsuariosFisicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosJuridicos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(26)", nullable: false),
                    RazaoSocial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeFantasia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cnpj = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    CnpjHash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataUltimoLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FkResponsavelId = table.Column<string>(type: "char(26)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosJuridicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosJuridicos_UsuariosFisicos_FkResponsavelId",
                        column: x => x.FkResponsavelId,
                        principalTable: "UsuariosFisicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(26)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FkPessoaFisicaId = table.Column<string>(type: "char(26)", nullable: true),
                    FkPessoaJuridicaId = table.Column<string>(type: "char(26)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contatos", x => x.Id);
                    table.CheckConstraint("CK_Contatos_Usuario_Exclusivo", "[FkPessoaFisicaId] IS NULL OR [FkPessoaJuridicaId] IS NULL");
                    table.ForeignKey(
                        name: "FK_Contatos_UsuariosFisicos_FkPessoaFisicaId",
                        column: x => x.FkPessoaFisicaId,
                        principalTable: "UsuariosFisicos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contatos_UsuariosJuridicos_FkPessoaJuridicaId",
                        column: x => x.FkPessoaJuridicaId,
                        principalTable: "UsuariosJuridicos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Endereco",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(26)", nullable: false),
                    TipoLogradouro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logradouro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Complemento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bairro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FkPessoaFisicaId = table.Column<string>(type: "char(26)", nullable: true),
                    FkPessoaJuridicaId = table.Column<string>(type: "char(26)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endereco", x => x.Id);
                    table.CheckConstraint("CK_Endereco_Usuario_Exclusivo", "[FkPessoaFisicaId] IS NULL OR [FkPessoaJuridicaId] IS NULL");
                    table.ForeignKey(
                        name: "FK_Endereco_UsuariosFisicos_FkPessoaFisicaId",
                        column: x => x.FkPessoaFisicaId,
                        principalTable: "UsuariosFisicos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Endereco_UsuariosJuridicos_FkPessoaJuridicaId",
                        column: x => x.FkPessoaJuridicaId,
                        principalTable: "UsuariosJuridicos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UsuariosFuncoes",
                columns: table => new
                {
                    FkPessoaFisicaId = table.Column<string>(type: "char(26)", nullable: false),
                    FkPessoaJuridicaId = table.Column<string>(type: "char(26)", nullable: false),
                    FkFuncaoId = table.Column<string>(type: "char(26)", nullable: false),
                    FkSetorId = table.Column<string>(type: "char(26)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosFuncoes", x => new { x.FkPessoaFisicaId, x.FkPessoaJuridicaId, x.FkFuncaoId, x.FkSetorId });
                    table.ForeignKey(
                        name: "FK_UsuariosFuncoes_Funcoes_FkFuncaoId",
                        column: x => x.FkFuncaoId,
                        principalTable: "Funcoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosFuncoes_Setores_FkSetorId",
                        column: x => x.FkSetorId,
                        principalTable: "Setores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosFuncoes_UsuariosFisicos_FkPessoaFisicaId",
                        column: x => x.FkPessoaFisicaId,
                        principalTable: "UsuariosFisicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosFuncoes_UsuariosJuridicos_FkPessoaJuridicaId",
                        column: x => x.FkPessoaJuridicaId,
                        principalTable: "UsuariosJuridicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_FkPessoaFisicaId",
                table: "Contatos",
                column: "FkPessoaFisicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_FkPessoaJuridicaId",
                table: "Contatos",
                column: "FkPessoaJuridicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Endereco_FkPessoaFisicaId",
                table: "Endereco",
                column: "FkPessoaFisicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Endereco_FkPessoaJuridicaId",
                table: "Endereco",
                column: "FkPessoaJuridicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Escolaridades_FkInstituicaoId",
                table: "Escolaridades",
                column: "FkInstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Escolaridades_FkUsuarioId",
                table: "Escolaridades",
                column: "FkUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienciasProfissionais_FkEmpresaId",
                table: "ExperienciasProfissionais",
                column: "FkEmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienciasProfissionais_FkUsuarioId",
                table: "ExperienciasProfissionais",
                column: "FkUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosFisicos_CpfHash",
                table: "UsuariosFisicos",
                column: "CpfHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosFuncoes_FkFuncaoId",
                table: "UsuariosFuncoes",
                column: "FkFuncaoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosFuncoes_FkPessoaJuridicaId",
                table: "UsuariosFuncoes",
                column: "FkPessoaJuridicaId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosFuncoes_FkSetorId",
                table: "UsuariosFuncoes",
                column: "FkSetorId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosJuridicos_CnpjHash",
                table: "UsuariosJuridicos",
                column: "CnpjHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosJuridicos_FkResponsavelId",
                table: "UsuariosJuridicos",
                column: "FkResponsavelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contatos");

            migrationBuilder.DropTable(
                name: "Endereco");

            migrationBuilder.DropTable(
                name: "Escolaridades");

            migrationBuilder.DropTable(
                name: "ExperienciasProfissionais");

            migrationBuilder.DropTable(
                name: "UsuariosFuncoes");

            migrationBuilder.DropTable(
                name: "Instituicoes");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Funcoes");

            migrationBuilder.DropTable(
                name: "Setores");

            migrationBuilder.DropTable(
                name: "UsuariosJuridicos");

            migrationBuilder.DropTable(
                name: "UsuariosFisicos");
        }
    }
}
