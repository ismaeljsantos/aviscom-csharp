using Aviscom.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviscom.Data.Configurations
{
    public class EnderecoConfiguration : BaseEntityConfiguration<Endereco>
    {
        public override void Configure(EntityTypeBuilder<Endereco> builder)
        {
            base.Configure(builder);
            builder.ToTable("Endereco");

            builder.Property(e => e.TipoLogradouro).IsRequired();
            builder.Property(e => e.Logradouro).IsRequired();
            builder.Property(e => e.Numero).IsRequired();
            builder.Property(e => e.Bairro).IsRequired();
            builder.Property(e => e.Cidade).IsRequired();
            builder.Property(e => e.Estado).IsRequired();
            builder.Property(e => e.Cep).IsRequired();
            

            // Índices opcionais
            builder.HasIndex(c => c.FkPessoaFisicaId);
            builder.HasIndex(c => c.FkPessoaJuridicaId);

            builder.ToTable(tb => tb.HasCheckConstraint(
            "CK_Endereco_Usuario_Exclusivo",
            "[FkPessoaFisicaId] IS NULL OR [FkPessoaJuridicaId] IS NULL"
        ));
        }
    }
}
