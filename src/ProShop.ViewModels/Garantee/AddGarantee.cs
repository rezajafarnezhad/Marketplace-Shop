using Microsoft.AspNetCore.Http;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProShop.ViewModels.Garantee;

public class AddGarantee
{
    [Display(Name = "عنوان گارانتی")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(120, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Title { get; set; }

    [Display(Name = "تعداد ماه گارانتی")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public byte MonthCount { get; set; }

    [Display(Name = "لوگوی برند")]
    [IsImage]
    [FileRequired]
    [MaxFileSize(3)]
    public IFormFile Picture { get; set; }




}