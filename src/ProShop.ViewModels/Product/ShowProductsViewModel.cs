using Microsoft.AspNetCore.Mvc.Rendering;
using ProShop.Common.Attributes;
using ProShop.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProShop.ViewModels.Product;

public class ShowProductsViewModel
{
    public PaginationViewModel pagination { get; set; } = new();
    public SearchProducts SearchProducts { get; set; } = new();
    public List<ShowProductViewModel> Products { get; set; } = new();
}

public class ShowProductViewModel
{
    [Display(Name = "شناسه")]
    public long Id { get; set; }

    [Display(Name = "عنوان فارسی محصول")]
    public string PersianTitle { get; set; }

    [Display(Name = " دسته بندی اصلی")]
    public string CategoryTitle { get; set; }

    [Display(Name = " کد محصول")]
    public int ProductCode { get; set; }


    [Display(Name = "تصویر اصلی محصول")]
    public string MainPicure { get; set; }

    [Display(Name = "فروشنده محصول")]

    public string SellerShopName { get; set; }

    [Display(Name = "عنوان برند محصول")]

    public string BrandFullTitle { get; set; }

    [Display(Name = "وضعیت محصول")]

    public ProductStatus Status { get; set; }


}

public class SearchProducts
{
    [ContainsSearch]
    [Display(Name = "نام فارسی کالا")]
    [MaxLength(200)]
    public string PersianTitle { get; set; }

    [EqualSearch]
    [Display(Name = "دسته بندی")]
    public long? MainCategoryId { get; set; }
    public List<SelectListItem> Categories { get; set; } = new();

    [Display(Name = "نام فروشنده محصول")]
    [MaxLength(200)]
    public string ShopName { get; set; }

    [EqualSearch]
    [Display(Name = " کد محصول")]
    public int? ProductCode { get; set; }


    [Display(Name = "وضعیت محصول")]
    public ProductStatus? Status { get; set; }

    [Display(Name = "مرتب سازی بر اساس")]
    public SortingOrder SortingOrder { get; set; }

    [Display(Name = "نمایش بر اساس")]
    public SortingProducts Sorting { get; set; }

    [Display(Name = "وضعیت حذف شده ها")]
    public DeletedStatus DeletedStatus { get; set; }
}

public enum SortingProducts
{
    [Display(Name = "شناسه")]
    Id,

    [Display(Name = "نام فارسی محصول")]
    PersianTitle,

    [Display(Name = "نام فروشنده محصول")]
    ShopName,

    [Display(Name = "نام فارسی برند")]
    BrandFa,
}