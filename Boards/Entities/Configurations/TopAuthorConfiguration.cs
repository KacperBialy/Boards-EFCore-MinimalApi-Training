using Boards.Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Boards.Entities.Configurations
{
    public class TopAuthorConfiguration : IEntityTypeConfiguration<TopAuthor>
    {
        public void Configure(EntityTypeBuilder<TopAuthor> eb)
        {
            eb.ToView("View_TopAuthors");
            eb.HasNoKey();
        }
    }
}
