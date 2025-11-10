using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NUlid;
using System.Linq;

namespace Aviscom.Data.Configurations
{
    public abstract class UlidEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            var converter = new ValueConverter<Ulid, string>(
                v => v.ToString(),
                v => Ulid.Parse(v));

            var nullableConverter = new ValueConverter<Ulid?, string>(
                v => v.HasValue ? v.Value.ToString() : (string)null!,
                v => v != null ? Ulid.Parse(v) : (Ulid?)null);

            var pkPropertyNames = builder.Metadata.FindPrimaryKey()
                                    ?.Properties.Select(p => p.Name)
                                    ?? Enumerable.Empty<string>();

            foreach (var property in builder.Metadata.GetProperties())
            {
                if (pkPropertyNames.Contains(property.Name))
                {
                    continue;
                }
                if (property.ClrType == typeof(Ulid))
                {
                    builder.Property(property.Name)
                        .HasColumnType("char(26)")
                        .HasConversion(converter);
                }
                else if (property.ClrType == typeof(Ulid?))
                {
                    builder.Property(property.Name)
                        .HasColumnType("char(26)")
                        .HasConversion(nullableConverter);
                }
            }
        }
    }
}