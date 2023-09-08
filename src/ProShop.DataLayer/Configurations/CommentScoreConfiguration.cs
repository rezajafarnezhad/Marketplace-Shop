using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class CommentScoreConfiguration : IEntityTypeConfiguration<CommentScore>
{
    public void Configure(EntityTypeBuilder<CommentScore> builder)
    {
        builder.ToTable("CommentScore");
        builder.Ignore(c => c.Id);
        builder.Ignore(c => c.IsDeleted);



        builder.HasKey(c => new { c.UserId, c.ProductCommentId });


        builder.HasOne(c => c.User)
            .WithMany(c => c.CommentScores)
            .HasForeignKey(c => c.UserId)
            .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

        builder.HasOne(c => c.ProductComment)
            .WithMany(c => c.CommentScores)
            .HasForeignKey(c => c.ProductCommentId)
            .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

    }
}