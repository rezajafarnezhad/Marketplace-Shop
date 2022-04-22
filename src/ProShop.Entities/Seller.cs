using ProShop.Entities.Identity;

namespace ProShop.Entities;

public class Seller : EntityBase, IAuditableEntity
{

    public long UserId { get; set; }
    public bool IsRealPerson { get; set; }

    #region Legal person
    public string CompanyName { get; set; }

   
    public string RegisterNumber { get; set; }

   
    public string EconomicCode { get; set; }

  
    public string SignatureOwners { get; set; }

   
    public string NationalId { get; set; }

    public CompanyType? CompanyType { get; set; }
    #endregion
    public int SellerCode { get; set; }

   
    public string ShopName { get; set; }

    
    public string AboutSeller { get; set; }

   
    public string Logo { get; set; }

    /// <summary>
    /// عکس کارت ملی
    /// </summary>
   
    public string IdCartPicture { get; set; }

    public string ShabaNumber { get; set; }
    public string Telephone { get; set; }
    public string Website { get; set; }

    public long ProvinceId { get; set; }

    public long CityId { get; set; }

   
    public string Address { get; set; }

   
    public string PostalCode { get; set; }

   
    public string Location { get; set; }

    public bool IsDocumentApproved { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDateTime { get; set; }


    public User User { get; set; }
    public ProvinceAndCity Province { get; set; }

    public ProvinceAndCity City { get; set; }

}