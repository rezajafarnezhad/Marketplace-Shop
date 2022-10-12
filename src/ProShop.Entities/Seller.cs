using System.ComponentModel.DataAnnotations;
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
    public string RejectReason { get; set; }
   

    public bool IsActive { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DocumentStatus DocumentStatus { get; set; }
    public User User { get; set; }
    public ProvinceAndCity Province { get; set; }

    public ProvinceAndCity City { get; set; }
    public List<Brand> Brands { get; set; }
    public List<Product> Products{ get; set; }
    public List<ProductVariant> ProductVariants{ get; set; }
    public List<Consignment> Consignments{ get; set; }
}

public enum DocumentStatus : byte
{
    [Display(Name = "در انتظار تایید اولیه")]
    AwaitingInitialApproval,

    [Display(Name = "تایید شده")]
    Confirmed,

    [Display(Name = "رد شده در حالت اولیه")]
    Rejected,

    [Display(Name = "در انتظار تایید فروشنده سیستم")]
    AwaitingApprovalSystemSeller,

    [Display(Name = "رد شده برای فروشنده  سیستم")]
    RejectedSystemSeller
}