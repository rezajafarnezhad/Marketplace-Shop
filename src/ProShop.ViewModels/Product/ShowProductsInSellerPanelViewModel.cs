using Microsoft.AspNetCore.Mvc.Rendering;
using ProShop.Common.Attributes;
using ProShop.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProShop.ViewModels.Product;

public class ShowProductsInSellerPanelViewModel
{
    public PaginationViewModel pagination { get; set; } = new();
    public SearchProductsInSellerPanel SearchProducts { get; set; } = new();
    public List<ShowProductInSellerPanelViewModel> Products { get; set; } = new();
}
public class ShowProductInSellerPanelViewModel
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

    [Display(Name = "عنوان برند محصول")]

    public string BrandFullTitle { get; set; }

    [Display(Name = "وضعیت محصول")]

    public ProductStatus Status { get; set; }


}

public class SearchProductsInSellerPanel
{
    [ContainsSearch]
    [Display(Name = "نام فارسی کالا")]
    [MaxLength(200)]
    public string PersianTitle { get; set; }

    [EqualSearch]
    [Display(Name = "دسته بندی")]
    public long? MainCategoryId { get; set; }
    public List<SelectListItem> Categories { get; set; } = new();

    [EqualSearch]
    [Display(Name = " کد محصول")]
    public int? ProductCode { get; set; }


    [Display(Name = "وضعیت محصول")]
    public ProductStatus? Status { get; set; }

    [Display(Name = "مرتب سازی بر اساس")]
    public SortingOrder SortingOrder { get; set; }

    [Display(Name = "نمایش بر اساس")]
    public SortingProductsInSellerPanel Sorting { get; set; }

    [Display(Name = "وضعیت حذف شده ها")]
    public DeletedStatus DeletedStatus { get; set; }
}

public enum SortingProductsInSellerPanel
{
    [Display(Name = "شناسه")]
    Id,

    [Display(Name = "نام فارسی محصول")]
    PersianTitle,

    [Display(Name = "نام فارسی برند")]
    BrandFa,
}