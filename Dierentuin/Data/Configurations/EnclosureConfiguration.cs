using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dierentuin.Models.Domain;

namespace Dierentuin.Data.Configurations
{
    public class EnclosureConfiguration : IEntityTypeConfiguration<Enclosure>
    {
        public void Configure(EntityTypeBuilder<Enclosure> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Size)
                .IsRequired();

            builder.Property(e => e.SecurityLevel)
                .IsRequired();

            builder.Property(e => e.Climate)
                .IsRequired();

            builder.Property(e => e.HabitatType)
                .IsRequired();
        }
    }
}
