using Aviscom.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviscom.Data.Configurations
{
    public class ExperienciaProfissionalConfiguration : BaseEntityConfiguration<ExperienciaProfissional>
    {
        public override void Configure(EntityTypeBuilder<ExperienciaProfissional> builder)
        {
            base.Configure(builder);
            builder.ToTable("ExperienciasProfissionais");
            builder.Property(e => e.Cargo).IsRequired();
        }
    }
}