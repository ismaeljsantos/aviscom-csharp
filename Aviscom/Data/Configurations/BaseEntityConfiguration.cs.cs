// Aviscom.Data/Configurations/BaseEntityConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Aviscom.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NUlid;

namespace Aviscom.Data.Configurations
{
    public class BaseEntityConfiguration<T> : UlidEntityConfiguration<T> where T : BaseEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            var converter = new ValueConverter<Ulid, string>(
                v => v.ToString(),
                v => Ulid.Parse(v));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnType("char(26)")
                .HasConversion(converter);

            builder.Property(e => e.DataCriacao)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.DataAtualizacao)
                .HasDefaultValueSql("GETUTCDATE()");

            base.Configure(builder);
        }
    }
}