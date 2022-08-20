using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;

namespace ProShop.ViewModels.Brands;

public class AddBrandBySelllerViewModel
{
    [Range(1,long.MaxValue,ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public long CategoryId { get; set; }

    [Display(Name = "نام فارسی برند")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [PageRemote(PageName = "Create", PageHandler = "CheckForTitleFa",
        HttpMethod = "POST",
        ErrorMessage = AttributesErrorMessages.RemoteMessage,
        AdditionalFields = ViewModelConstants.AntiForgeryToken)]
    public string TitleFa { get; set; }

    [Display(Name = "نام انگلیسی برند")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [LtrDir]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [PageRemote(PageName = "Create", PageHandler = "CheckForTitleEn",
        HttpMethod = "POST",
        ErrorMessage = AttributesErrorMessages.RemoteMessage,
        AdditionalFields = ViewModelConstants.AntiForgeryToken)]
    public string TitleEn { get; set; }

    [Display(Name = "شرح برند")]
    [MakeTinyMceRequired]
    public string Description { get; set; }

    [Display(Name = "نوع برند")]
    public bool IsIranianBrand { get; set; }

    [Display(Name = "لوگوی برند")]
    [FileRequired]
    [MaxFileSize(2)]
    [IsImage]
    public IFormFile LogoPicture { get; set; }

    [Display(Name = "برگه ثبت برند")]
    [MaxFileSize(3)]
    [IsImage]
    public IFormFile BrandRegistrationPicture { get; set; }

    [Display(Name = "لینک سایت قوه قضاییه")]
    [LtrDir]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string JudiciaryLink { get; set; }

    [Display(Name = "لینک سایت معتبر خارجی")]
    [LtrDir]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string BrandLinkEn { get; set; }
}