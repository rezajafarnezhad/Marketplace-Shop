namespace ProShop.Entities;

public class ProvinceAndCity : EntityBase, IAuditableEntity
{
    #region Properties

    public string Title { get; set; }

    public long? ParentId { get; set; }

    #endregion

    #region Relations

    public ProvinceAndCity Parent { get; set; }
    public ICollection<Seller> SellerProvinces { get; set; }
    public ICollection<Seller> SellerCities { get; set; }
    
    public ICollection<Address> AddressProvinces { get; set; }
    public ICollection<Address> AddressCities { get; set; }
    #endregion
}