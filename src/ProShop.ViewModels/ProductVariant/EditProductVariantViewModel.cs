using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using System.ComponentModel.DataAnnotations;


namespace ProShop.ViewModels.ProductVariant;


public class EditProductVariantViewModel
{

    public long Id { get; set; }

    public string Slug { get; set; }

    public int ProductCode { get; set; }

    [DivisibleBy10]
    [Required(ErrorMessage =AttributesErrorMessages.RequiredMessage)]
    [Display(Name = "قیمت")]
    [Range(1, 20000000000, ErrorMessage = AttributesErrorMessages.RangeMessage)]
    public int Price { get; set; }

    public string ProductTitle { get; set; }

    public string ProductCategoryTitle { get; set; }

    public bool CategoryIsVariantColor { get; set; }

    public string ProductBrandFullTitle { get; set; }

    public byte CommissionPercentage { get; set; }

    public string MainPicture { get; set; }
    public bool IsDiscountActive { get; set; }


}



public class AddEditDiscountViewModel
{

    public long Id { get; set; }

    public string Slug { get; set; }

    public int ProductCode { get; set; }

   
    public int Price { get; set; }

    public string ProductTitle { get; set; }

    public string ProductCategoryTitle { get; set; }

    public bool CategoryIsVariantColor { get; set; }

    public string ProductBrandFullTitle { get; set; }

    public byte CommissionPercentage { get; set; }

    public string MainPicture { get; set; }

    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [DivisibleBy10]
    [Display(Name = "قیمت با تخفیف")]
    [Range(1, 20000000000, ErrorMessage = AttributesErrorMessages.RangeMessage)]
    public int OffPrice { get; set; }

    [Display(Name = "درصد تخفیف")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]

    public byte? offPercentage { get; set; }
    

    [Display(Name = "تاریخ شروع تخفیف")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]

    public string StartDateTime { get; set; }
    public string StartDateTimeEn { get; set; }

    [Display(Name = "تاریخ پایان تخفیف")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]


    public string EndDateTime { get; set; }
    public string EndDateTimeEn { get; set; }

}
