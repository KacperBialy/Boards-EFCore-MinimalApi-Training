using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Boards.Entities.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.OwnsOne(a =>
                a.Coordinate, onb =>
                    {
                        onb.Property(c => c.Lattitude).HasPrecision(18, 7);
                        onb.Property(c => c.Longitude).HasPrecision(18, 7);
                    });

            builder.OwnsOne(a => a.Coordinate, onb =>
             {
                 onb.Property(c => c.Lattitude).HasPrecision(18, 7);
                 onb.Property(c => c.Longitude).HasPrecision(18, 7);
             });
        }
    }
}
