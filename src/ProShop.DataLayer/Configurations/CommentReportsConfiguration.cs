using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class CommentReportsConfiguration : IEntityTypeConfiguration<CommentsReports>
{
    public void Configure(EntityTypeBuilder<CommentsReports> builder)
    {
        builder.ToTable("CommentsReports");
        builder.Ignore(c => c.Id);
        builder.Ignore(c => c.IsDeleted);



        builder.HasKey(c => new { c.UserId, c.ProductCommentId });


        builder.HasOne(c => c.User)
            .WithMany(c => c.CommentsReports)
            .HasForeignKey(c => c.UserId)
            .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

        builder.HasOne(c => c.ProductComment)
            .WithMany(c => c.CommentsReports)
            .HasForeignKey(c => c.ProductCommentId)
            .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

    }
}