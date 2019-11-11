using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSample.Data.Entities;

namespace WebSample.Data.EntityConfigurations
{
    public class UniverseConfiguration : IEntityTypeConfiguration<Universe>
    {
        public void Configure(EntityTypeBuilder<Universe> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnName("id");

            builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(255);

            builder.ToTable("universes");
        }
    }
}
