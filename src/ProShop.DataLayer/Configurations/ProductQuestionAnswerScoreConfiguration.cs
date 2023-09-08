using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ProductQuestionAnswerScoreConfiguration : IEntityTypeConfiguration<ProductQuestionAnswerScore>
{
    public void Configure(EntityTypeBuilder<ProductQuestionAnswerScore> builder)
    {
        builder.ToTable("ProductQuestionAnswerScore");
        builder.Ignore(c => c.Id);
        builder.Ignore(c => c.IsDeleted);



        builder.HasKey(c => new { c.UserId, c.AnswerId });


        builder.HasOne(c => c.User)
            .WithMany(c => c.ProductQuestionAnswerScore)
            .HasForeignKey(c => c.UserId)
            .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

        builder.HasOne(c => c.Answer)
            .WithMany(c => c.ProductQuestionAnswerScore)
            .HasForeignKey(c => c.AnswerId)
            .OnDelete(deleteBehavior: DeleteBehavior.NoAction);

    }
}