namespace ProShop.Entities;
public class Category : EntityBase, IAuditableEntity
{
    public string Title { get; set; }
    public long? ParentId { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string Picture { get; set; }
    public bool IsShowInMenus { get; set; } = false;
    public bool CanAddFakeProduct { get; set; } = false;
    public bool IsVariantColor { get; set; } = false;


    public Category ParentCategory { get; set; }
    public ICollection<Category> ChildCategory { get; set; }
    public ICollection<CategoryFeature> CategoryFeatures { get; set; }
    public ICollection<CategoryBrand> CategoryBrands { get; set; } = new List<CategoryBrand>();
    public ICollection<FeatureConstantValue> FeatureConstantValues { get; set; } = new List<FeatureConstantValue>();
    public ICollection<ProductCategory> productCategories { get; set; }
    public ICollection<Product> Products { get; set; }
    public ICollection<CategoryVarieant> categoryVarieants { get; set; }
    
}
