using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using ProShop.Entities;
using ProShop.Entities.Identity;

namespace ProShop.ViewModels.Sellers;

public class CreateSellerViewModel
{
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Display(Name = "شماره موبایل")]
    public string PhoneNumber { get; set; }

    [Display(Name = "نام")]
    [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
        ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
        ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string LastName { get; set; }

    [Display(Name = "کد ملی")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(11, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [LtrDir]
    public string NationalCode { get; set; }

    [Display(Name = "تاریخ تولد")]
    [LtrDir]
    [RegularExpression(@"^۱۳[۰-۸][۰-۹]\/(۰[۱-۹]|۱[۰-۲])\/(۰[۱-۹]|[۱۲][۰-۹]|۳[۰۱])$", ErrorMessage = "سن باید بین ۱۸ و ۱۰۰ سال باشد")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public string BirthDate { get; set; }
    public string? BirthDateEn { get; set; }

    [Display(Name = "جنسیت")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public Gender? Gender { get; set; }

    public bool IsLegalPerson { get; set; } = false;

    #region Legal person

    [Display(Name = "نام شرکت")]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? CompanyName { get; set; }

    [Display(Name = "شماره ثبت شرکت")]
    [RegularExpression(@"^[\d]+$", ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    [LtrDir]
    [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? RegisterNumber { get; set; }

    [Display(Name = "کد اقتصادی")]
    [RegularExpression(@"^[\d]+$", ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    [LtrDir]
    [MaxLength(12, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? EconomicCode { get; set; }

    [Display(Name = "نام افراد دارای حق امضا")]
    [MaxLength(300, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? SignatureOwners { get; set; }

    [Display(Name = "شناسه ملی")]
    [LtrDir]
    [MaxLength(30, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [RegularExpression(@"^[\d]+$",
        ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public string? NationalId { get; set; }

    public CompanyType? CompanyType { get; set; }

    #endregion

    [Display(Name = "نام فروشگاه")]
    [PageRemote(
        PageName = "CreateSeller",
        PageHandler = "CheckForShopName",
        HttpMethod = "GET",
        ErrorMessage = AttributesErrorMessages.RemoteMessage)]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string ShopName { get; set; }

    [Display(Name = "درباره فروشگاه")]
    public string? AboutSeller { get; set; }

    [Display(Name = "لوگو فروشگاه")]
    [IsImage("لوگو فروشگاه")]
    [MaxFileSize("لوگو فروشگاه", 1)]
    public IFormFile? LogoFile { get; set; }

    /// <summary>
    /// عکس کارت ملی
    /// </summary>
    [Display(Name = "تصویر کارت ملی")]
    [FileRequired("تصویر کارت ملی")]
    [IsImage("تصویر کارت ملی")]
    [MaxFileSize("تصویر کارت ملی", 1)]
    public IFormFile IdCartPictureFile { get; set; }

    [Display(Name = "شماره شبا")]
    [PageRemote(
        PageName = "CreateSeller",
        PageHandler = "CheckForShabaNumber",
        HttpMethod = "GET",
        ErrorMessage = AttributesErrorMessages.RemoteMessage)]
    [LtrDir]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(24, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string ShabaNumber { get; set; }


    [Display(Name = "شماره تلفن ثابت")]
    [LtrDir]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [RegularExpression(@"[\d]{11}", ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    [MaxLength(11, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Telephone { get; set; }

    [Display(Name = "آدرس وبسایت")]
    [LtrDir]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? Website { get; set; }

    [Display(Name = "استان")]
    [Range(1, long.MaxValue, ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public long ProvinceId { get; set; }

    [Display(Name = "شهرستان")]
    [Range(1, long.MaxValue, ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public long CityId { get; set; }

    [Display(Name = "آدرس کامل")]
    [Required(ErrorMessage = "آدرس را وارد نمایید")]
    [MaxLength(300, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Address { get; set; }

    [Display(Name = "کد پستی")]
    [LtrDir]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(10, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [RegularExpression(@"[\d]{10}", ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public string PostalCode { get; set; }

    public List<SelectListItem> Provinces { get; set; } = new();

    [Display(Name = "قوانین و قرارداد را  به صورت کامل خوانده و قبول دارم")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "شما باید قوانین و مقرررات را تایید نمایید")]
    public bool AcceptToTheTerms { get; set; }
}