using Aviscom.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviscom.Data.Configurations
{
    public class UsuarioPessoaJuridicaConfiguration : BaseEntityConfiguration<UsuarioPessoaJuridica>
    {
        public override void Configure(EntityTypeBuilder<UsuarioPessoaJuridica> builder)
        {
            base.Configure(builder);
            builder.ToTable("UsuariosJuridicos");

            // Configurações específicas de PessoaJuridica
            builder.Property(u => u.RazaoSocial).IsRequired();
            builder.Property(u => u.Cnpj).IsRequired().HasMaxLength(14);
            builder.Property(u => u.CnpjHash).IsRequired();
            builder.Property(u => u.SenhaHash).IsRequired();
        }
    }
}