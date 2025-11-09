// Aviscom.Data/Configurations/ContatoConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aviscom.Models.Usuario;

namespace Aviscom.Data.Configurations
{
    public class ContatoConfiguration : BaseEntityConfiguration<Contato>
    {
        public override void Configure(EntityTypeBuilder<Contato> builder)
        {
            base.Configure(builder);

            builder.ToTable("Contatos");

            builder.Property(c => c.Tipo).IsRequired();
            builder.Property(c => c.Valor).IsRequired();

            // Índices opcionais
            builder.HasIndex(c => c.FkPessoaFisicaId);
            builder.HasIndex(c => c.FkPessoaJuridicaId);

            builder.ToTable(tb => tb.HasCheckConstraint(
                "CK_Contatos_Usuario_Exclusivo",
                "[FkPessoaFisicaId] IS NULL OR [FkPessoaJuridicaId] IS NULL"
            ));
        }
    }
}