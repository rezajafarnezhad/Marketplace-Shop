namespace ProShop.Entities;

public class CategoryFeature :EntityBase, IAuditableEntity
{
    public long FeatureId { get; set; }
    public long CategoryId { get; set; }

    public Feature Feature { get; set; }
    public Category Category { get; set; }

}