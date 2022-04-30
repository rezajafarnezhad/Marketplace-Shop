namespace ProShop.Entities;

public class CategoryBrand :EntityBase, IAuditableEntity
{
    public long BrandId { get; set; }
    public long CategoryId { get; set; }

    public Brand Brand { get; set; }
    public Category Category { get; set; }

}