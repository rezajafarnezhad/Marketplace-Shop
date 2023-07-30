using System.ComponentModel.DataAnnotations;
using ProShop.Common.Constants;

namespace ProShop.ViewModels.Veriants;

public class AddVariantInPanelAdmin
{
    [Required(ErrorMessage =AttributesErrorMessages.RequiredMessage)]
    [Display(Name ="مقدار")]
    public string Value { get; set; }
    [Display(Name ="رنگ / سایز")]
    public bool IsColor { get; set; }
    [Display(Name ="کد رنگ")]
    public string ColorCode { get; set; }
    public bool IsConfirmed { get; set; }

}