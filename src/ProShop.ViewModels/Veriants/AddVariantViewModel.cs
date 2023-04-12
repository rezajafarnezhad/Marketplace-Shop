using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using System.ComponentModel.DataAnnotations;


namespace ProShop.ViewModels.Veriants;

public class AddVariantViewModel
{

    public long ProductId { get; set; }

    public string Slug { get; set; }

    public int ProductCode { get; set; }
    
    
    [Display(Name = "تنوع")]
    [Range(1, long.MaxValue, ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public long VariantId { get; set; }


    [Display(Name = "گارانتی")]
    [Required(ErrorMessage =AttributesErrorMessages.RequiredMessage)]
    [Range(1, long.MaxValue, ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public long GaranteeId { get; set; }


    public List<SelectListItem> Garantees { get; set; } = new();



    [Display(Name = "قیمت")]
    [Range(1, 20000000000, ErrorMessage = AttributesErrorMessages.RangeMessage)]
    [DivisibleBy10]
    public int Price { get; set; } 
    
    [Display(Name = "حداکثر تعداد در سبد خرید")]
    [Range(1, short.MaxValue, ErrorMessage = AttributesErrorMessages.RangeMessage)]
    public short MaxCountInCart { get; set; }

    public string ProductTitle { get; set; }

    public string CategoryTitle { get; set; }

    public bool? CategoryIsVariantColor { get; set; }

    public string BrandFullTitle { get; set; }

    public byte CommissionPercentage { get; set; }

    public string MainPicture { get; set; }

    public List<ShowCategoryVariantInAddVariantViewModel> Variants { get; set; }
}

public class ShowCategoryVariantInAddVariantViewModel
{
    public long VariantId { get; set; }
    public string VariantValue { get; set; }
    public bool? VariantIsColor { get; set; }
    public string VariantColorCode { get; set; }
}
