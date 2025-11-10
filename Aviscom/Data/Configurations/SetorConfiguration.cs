using Aviscom.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviscom.Data.Configurations
{
    public class SetorConfiguration : BaseEntityConfiguration<Setor>
    {
        public override void Configure(EntityTypeBuilder<Setor> builder)
        {
            base.Configure(builder);
            builder.ToTable("Setores");
            builder.Property(e => e.Nome).IsRequired();
        }
    }
}