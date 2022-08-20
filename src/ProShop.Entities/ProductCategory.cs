namespace ProShop.Entities;

public class ProductCategory : EntityBase ,IAuditableEntity
{

    public long ProductId { get; set; }
    public long CategoryId { get; set; }



    public Category Category { get; set; }
    public Product Product { get; set; }

}