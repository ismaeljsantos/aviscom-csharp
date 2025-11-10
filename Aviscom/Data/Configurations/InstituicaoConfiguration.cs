using Aviscom.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviscom.Data.Configurations
{
    public class InstituicaoConfiguration : BaseEntityConfiguration<Instituicao>
    {
        public override void Configure(EntityTypeBuilder<Instituicao> builder)
        {
            base.Configure(builder);
            builder.ToTable("Instituicoes");
            builder.Property(e => e.Nome).IsRequired(); //
        }
    }
}