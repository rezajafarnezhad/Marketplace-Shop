using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;

namespace ProShop.ViewModels.Brands;

public class EditBrandViewModel
{

    public long Id { get; set; }

    [Display(Name = "نام فارسی برند")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [PageRemote(PageName = "Index", PageHandler = "CheckForEditTitleFa",
        HttpMethod = "POST",
        ErrorMessage = AttributesErrorMessages.RemoteMessage,
        AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id))]
    public string TitleFa { get; set; }

    [Display(Name = "نام انگلیسی برند")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [LtrDir]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [PageRemote(PageName = "Index", PageHandler = "CheckForEditTitleEn",
        HttpMethod = "POST",
        ErrorMessage = AttributesErrorMessages.RemoteMessage,
        AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id))]
    public string TitleEn { get; set; }

    [Display(Name = "شرح برند")]
    [MakeTinyMceRequired]
    public string Description { get; set; }

    [Display(Name = "نوع برند")]
    public bool IsIranianBrand { get; set; }

    [Display(Name = "لوگوی برند")]
    [MaxFileSize(2)]
    [IsImage]
    public IFormFile? NewLogoPicture { get; set; }

    [Display(Name = "برگه ثبت برند")]
    [MaxFileSize(3)]
    [IsImage]
    public IFormFile? NewBrandRegistrationPicture { get; set; }

    [Display(Name = "لینک سایت قوه قضاییه")]
    [LtrDir]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string JudiciaryLink { get; set; }

    [Display(Name = "لینک سایت معتبر خارجی")]
    [LtrDir]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string BrandLinkEn { get; set; }

    public string LogoPicture { get; set; }
    public string BrandRegistrationPicture { get; set; }
}