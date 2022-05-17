namespace ProShop.Entities;

public class FeatureConstantValue :EntityBase, IAuditableEntity
{
    public long FeatureId { get; set; }
    public long CategoryId { get; set; }
    public string Value { get; set; }

    public Feature Feature { get; set; }
    public Category Category { get; set; }

}