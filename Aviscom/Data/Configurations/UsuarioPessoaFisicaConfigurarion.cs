using Aviscom.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviscom.Data.Configurations
{
    public class UsuarioPessoaFisicaConfiguration : BaseEntityConfiguration<UsuarioPessoaFisica>
    {
        public override void Configure(EntityTypeBuilder<UsuarioPessoaFisica> builder)
        {
            base.Configure(builder);
            builder.ToTable("UsuariosFisicos");

            // Configurações específicas de PessoaFisica
            builder.Property(u => u.Nome).IsRequired();
            builder.Property(u => u.CpfCriptografado).IsRequired();
            builder.Property(u => u.CpfHash).IsRequired();
            builder.Property(u => u.Sexo).IsRequired();
            builder.Property(u => u.DataNascimento).IsRequired();
            builder.Property(u => u.NomeMae).IsRequired();
            builder.Property(u => u.SenhaHash).IsRequired();

            // Relações (Exemplo de como definir a relação de EmpresasResponsavel)
            // Nota: O EF deve pegar isso automaticamente, mas é bom ser explícito
            builder.HasMany(pf => pf.EmpresasResponsavel)
                   .WithOne(pj => pj.Responsavel)
                   .HasForeignKey(pj => pj.FkResponsavelId)
                   .OnDelete(DeleteBehavior.Restrict); // Já definido no Context
        }
    }
}