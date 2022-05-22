using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Boards.Entities.Configurations
{
    public class EpicConfiguration : IEntityTypeConfiguration<Epic>
    {
        public void Configure(EntityTypeBuilder<Epic> builder)
        {
            builder.Property(wi => wi.EndDate)
                .HasPrecision(3);
        }
    }
}
