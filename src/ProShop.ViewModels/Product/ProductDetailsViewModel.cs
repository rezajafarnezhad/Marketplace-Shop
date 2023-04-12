using System.ComponentModel.DataAnnotations;
using ProShop.Common.Attributes;
using ProShop.Entities;

namespace ProShop.ViewModels.Product;

public class ProductDetailsViewModel
{

    public long Id { get; set; }

    [Display(Name = "دلیل رد شدن محصول")]
    [MakeTinyMceRequired]
    public string RejectReason { get; set; }

    [Display(Name = " کد محصول")]
    public int ProductCode { get; set; }

    [Display(Name = "نام فارسی محصول")]
    public string PersianTitle { get; set; }

    [Display(Name = "نام انگلیسی محصول")]
    public string EnglishTitle { get; set; }

    public string Slug { get; set; }

    public bool IsFake { get; set; }

    [Display(Name = "وزن بسته بندی")]
    public int PackWeight { get; set; }

    [Display(Name = "طول بسته بندی")]
    public int PackLength { get; set; }

    [Display(Name = "عرض بسته بندی")]
    public int PackWidth { get; set; }

    [Display(Name = "ارتفاع بسته بندی")]
    public int PackHeight { get; set; }

    [Display(Name = "توضیحات مختصر محصول")]
    public string ShortDescription { get; set; }

    [Display(Name = "بررسی تخصصی محصول")]
    public string SpecialCheck { get; set; }

    [Display(Name = "نام فروشگاه")]
    public string SellerShopName { get; set; }

    [Display(Name = "برند محصول")]
    public string BrandFullTitle { get; set; }

    public ProductStatus Status { get; set; }

    [Display(Name = "ابعاد")]
    public ProductDimensions Dimensions { get; set; }

    public string CategoryTitle { get; set; }

    public List<ProductMediaForDetailProductViewModel> ProductMedia { get; set; }
    public List<ProductFeatureForDetailProductViewModel> ProductFeatures { get; set; }
}


public class ProductMediaForDetailProductViewModel
{
    public string FileName { get; set; }
    public bool IsVideo { get; set; }
}

public class ProductFeatureForDetailProductViewModel
{
    public string FeatureTitle { get; set; }

    public string Value { get; set; }
}
