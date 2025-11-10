using Aviscom.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviscom.Data.Configurations
{
    public class EscolaridadeConfiguration : BaseEntityConfiguration<Escolaridade>
    {
        public override void Configure(EntityTypeBuilder<Escolaridade> builder)
        {
            base.Configure(builder);
            builder.ToTable("Escolaridades");
            builder.Property(e => e.Tipo).IsRequired();
        }
    }
}