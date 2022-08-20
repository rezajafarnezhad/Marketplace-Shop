namespace ProShop.Entities;

public class Product : EntityBase , IAuditableEntity
{

    public string PersianTitle { get; set; }
    public string EnglishTitle { get; set; }
    public bool IsFake { get; set; }
    public int PackWeight { get; set; }
    public int PackLength { get; set; }
    public int Packwidth { get; set; }
    public int PackHeight { get; set; }
    public string ShortDescription { get; set; }
    public string SpecialCheck { get; set; }

    public long BrandId { get; set; }



    public ICollection<ProductCategory> productCategories { get; set; }
    public ICollection<ProductMedia> ProductMedia  { get; set; }
    public ICollection<ProductFeature> ProductFeatures { get; set; }

    public Brand Brand { get; set; }
}
