using Aviscom.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviscom.Data.Configurations
{
    public class EmpresaConfiguration : BaseEntityConfiguration<Empresa>
    {
        public override void Configure(EntityTypeBuilder<Empresa> builder)
        {
            base.Configure(builder);
            builder.ToTable("Empresas");
            builder.Property(e => e.Nome).IsRequired();
        }
    }
}