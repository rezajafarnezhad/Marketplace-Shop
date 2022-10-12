using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace ProShop.ViewModels.ProductVariant;

public class ShowProductVariantViewModel
{
    [Display(Name ="مقدار تنوع")]
    public string VariantValue { get; set; }
    public bool VariantIsColor { get; set; }
    public string VariantColorCode { get; set; }

    [Display(Name = "گارانتی")]

    public string GatanteeFullTitle { get; set; }

    [Display(Name = "قیمت")]

    public int Price { get; set; }
    
    [Display(Name = "کد تنوع محصول")]

    public int VariantCode { get; set; }
}
