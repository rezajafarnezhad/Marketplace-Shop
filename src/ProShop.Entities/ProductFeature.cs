namespace ProShop.Entities;

public class ProductFeature:EntityBase , IAuditableEntity
{
    public long ProductId { get; set; }
    public long FeatureId { get; set; }
    
    public string Value { get; set; }


    public Product Product { get; set; }
    public Feature Feature { get; set; }



}