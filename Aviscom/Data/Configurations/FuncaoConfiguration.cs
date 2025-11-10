using Aviscom.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviscom.Data.Configurations
{
    public class FuncaoConfiguration : BaseEntityConfiguration<Funcao>
    {
        public override void Configure(EntityTypeBuilder<Funcao> builder)
        {
            base.Configure(builder);
            builder.ToTable("Funcoes");
            builder.Property(e => e.Titulo).IsRequired();
        }
    }
}