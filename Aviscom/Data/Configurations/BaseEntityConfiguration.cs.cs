// Aviscom.Data/Configurations/BaseEntityConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aviscom.Models;

namespace Aviscom.Data.Configurations
{
    public class BaseEntityConfiguration<T> : UlidEntityConfiguration<T> where T : BaseEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.DataCriacao)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.DataAtualizacao)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}