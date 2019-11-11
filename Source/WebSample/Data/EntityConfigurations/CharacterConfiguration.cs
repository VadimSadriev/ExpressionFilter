using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSample.Data.Entities;
using WebSample.Data.Enums;

namespace WebSample.Data.EntityConfigurations
{
    public class CharacterConfiguration : IEntityTypeConfiguration<Character>
    {
        public void Configure(EntityTypeBuilder<Character> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id");

            builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(255);

            builder.Property(p => p.IsDeleted).HasColumnName("is_deleted").IsRequired(false);

            builder.Property(p => p.Type).HasColumnName("type")
                .HasConversion<string>()
                .HasDefaultValue(CharacterType.Mage);

            builder.Property(p => p.UniverseId).IsRequired(false).HasColumnName("universe_id");

            builder.HasOne(p => p.Universe)
                .WithMany(p => p.Characters)
                .HasForeignKey(p => p.UniverseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("characters");
        }
    }
}
