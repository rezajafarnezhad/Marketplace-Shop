using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProShop.Common.Constants;

namespace ProShop.ViewModels.FeatureConstantValue;

public class AddFeatureConstantValue
{
    [Display(Name = "دسته بندی")]
    [Range(1,long.MaxValue,ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public long CategoryId { get; set; }

    [Display(Name = "ویژگی ها")]
    [Range(1, long.MaxValue, ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public long FeatureId { get; set; }

    [Display(Name = "مقدار")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Value { get; set; }

    public List<SelectListItem> Categories { get; set; }

}