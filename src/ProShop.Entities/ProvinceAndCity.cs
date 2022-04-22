namespace ProShop.Entities;

public class ProvinceAndCity : EntityBase, IAuditableEntity
{
    #region Properties

    public string Title { get; set; }

    public long? ParentId { get; set; }

    #endregion

    #region Relations

    public ProvinceAndCity Parent { get; set; }
    public ICollection<Seller> Provinces { get; set; }
    public ICollection<Seller> Cities { get; set; }
    #endregion
}