using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProShop.Common.Constants;

namespace ProShop.ViewModels.Product;

public class AddProductViewModel
{
    [Display(Name = "برند محصول")]
    [Range(1,long.MaxValue,ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public long BrandId { get; set; }

    [Display(Name = "اصالت محصول")]
    public bool IsFake { get; set; }

    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Display(Name = "وزن بسته بندی")]
    public int PackWeight { get; set; }

    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Display(Name = " طول بسته بندی")]
    public int PackLength { get; set; }

    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Display(Name = "عرض بسته بندی")]
    public int PackWidth { get; set; }

    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Display(Name = "ارتفاع بسته بندی")]
    public int PackHeight { get; set; }

    [Display(Name = "توضیحات کوتاه")]
    public string ShortDescription { get; set; }
    
    [Display(Name = "بررسی تخصصی")]
    public string SpecialtyCheck { get; set; }
}