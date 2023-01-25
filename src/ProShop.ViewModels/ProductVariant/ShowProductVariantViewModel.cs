using ProShop.Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace ProShop.ViewModels.ProductVariant;

public class ShowProductVariantViewModel
{

    public long Id { get; set; }

    [Display(Name ="مقدار تنوع")]
    public string VariantValue { get; set; }
    public bool? VariantIsColor { get; set; }
    public string VariantColorCode { get; set; }

    [Display(Name = "گارانتی")]

    public string GatanteeFullTitle { get; set; }

    [Display(Name = "قیمت")]

    public int Price { get; set; }
    
    [Display(Name = "کد تنوع محصول")]

    public int VariantCode { get; set; }

    [Display(Name = "قیمت با تخفیف")]
    [Range(1, 20000000000, ErrorMessage = AttributesErrorMessages.RangeMessage)]
    public int? OffPrice { get; set; }

    [Display(Name = "درصد تخفیف")]
    public byte? offPercentage { get; set; }
    public int FinalPrice => OffPrice ?? Price;

    [Display(Name = "تاریخ شروع تخفیف")]
    public string StartDateTime { get; set; }

    [Display(Name = "تاریخ پایان تخفیف")]

    public string EndDateTime { get; set; }
}
