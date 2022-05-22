using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Boards.Entities.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {


        public void Configure(EntityTypeBuilder<Comment> eb)
        {
            eb.Property(x => x.CreatedDate)
                .HasDefaultValueSql("getutcdate()");

            eb.Property(x => x.UpdatedDate)
                .ValueGeneratedOnUpdate();
        }
    }
}
