using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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
    public long SelerId { get; set; }
    public long MainCategoryId { get; set; }
    public string Slug { get; set; }
    public string RejectReason { get; set; }

    public ProductStatus Status { get; set; }


    public ICollection<ProductCategory> productCategories { get; set; } = new List<ProductCategory>();
    public ICollection<ProductMedia> ProductMedia  { get; set; } = new List<ProductMedia>();
    public ICollection<ProductFeature> ProductFeatures { get; set; } = new List<ProductFeature>();

    public Brand Brand { get; set; }
    public Seller Seller { get; set; }
    public Category Category{ get; set; }
}


public enum ProductStatus : byte
{
    [Display(Name = "در انتظار تایید اولیه")]
    AwaitingInitialApproval,

    [Display(Name = "تایید شده")]
    Confirmed,

    [Display(Name = "رد شده در حالت اولیه")]
    Rejected,

}