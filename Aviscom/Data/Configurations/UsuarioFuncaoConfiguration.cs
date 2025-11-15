using Aviscom.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviscom.Data.Configurations
{
    public class UsuarioFuncaoConfiguration : BaseEntityConfiguration<UsuarioFuncao>
    {
        public override void Configure(EntityTypeBuilder<UsuarioFuncao> builder)
        {
            // 1. Configura a BaseEntity (para o 'Id' Ulid)
            base.Configure(builder);
            builder.ToTable("UsuariosFuncoes");

            // 2. Garante que ou PF ou PJ é nulo (como em Endereco/Contato)
            builder.ToTable(tb => tb.HasCheckConstraint(
                "CK_UsuarioFuncao_Usuario_Exclusivo",
                "[FkPessoaFisicaId] IS NULL OR [FkPessoaJuridicaId] IS NULL"
            ));

            // 3. (Opcional mas recomendado) Índice para evitar duplicados
            builder.HasIndex(uf => new {
                uf.FkPessoaFisicaId,
                uf.FkPessoaJuridicaId,
                uf.FkFuncaoId,
                uf.FkSetorId
            }).IsUnique();
        }
    }
}
