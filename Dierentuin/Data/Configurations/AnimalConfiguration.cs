using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dierentuin.Models.Domain;

namespace Dierentuin.Data.Configurations
{
    public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
    {
        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Species)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.SpaceRequirement)
                .IsRequired();

            builder.HasOne(a => a.Category)
                .WithMany(c => c.Animals)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Enclosure)
                .WithMany(e => e.Animals)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
