using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProShop.Entities;

namespace ProShop.DataLayer.Configurations;

public class ProductQuestionAndAnswerConfiguration : IEntityTypeConfiguration<ProductQuestionAndAnswer>
{
    public void Configure(EntityTypeBuilder<ProductQuestionAndAnswer> builder)
    {
        builder.ToTable("ProductQuestionAndAnswer");
      
        builder.HasKey(c =>c.Id);
        builder.Property(c=>c.Body).IsRequired().HasMaxLength(500);

        builder.HasOne(c => c.Product)
            .WithMany(c => c.ProductsQuestionsAndAnswers)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(deleteBehavior: DeleteBehavior.NoAction);


        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Answers)
            .HasForeignKey(c => c.IsParent)
            .OnDelete(deleteBehavior: DeleteBehavior.NoAction);
        
    }
}