using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;

namespace ProShop.ViewModels.Product;

public class AddProductViewModel
{

    public long MainCategoryId { get; set; }

    [Display(Name = "برند محصول")]
    [Range(1,long.MaxValue,ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public long BrandId { get; set; }

    [Display(Name = "اصالت محصول")]
    public bool? IsFake { get; set; }

    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Display(Name = "وزن بسته بندی")]
    [Range(1, 1000000, ErrorMessage = AttributesErrorMessages.RangeMessage)]
    public int PackWeight { get; set; }

    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Display(Name = " طول بسته بندی")]
    [Range(1, 20000, ErrorMessage = AttributesErrorMessages.RangeMessage)]
    public int PackLength { get; set; }

    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Display(Name = "عرض بسته بندی")]
    [Range(1, 20000, ErrorMessage = AttributesErrorMessages.RangeMessage)]
    public int PackWidth { get; set; }

    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Display(Name = "ارتفاع بسته بندی")]
    [Range(1, 10000, ErrorMessage = AttributesErrorMessages.RangeMessage)]
    public int PackHeight { get; set; }

    [Display(Name = "توضیحات کوتاه")]
    public string ShortDescription { get; set; }
    
    [Display(Name = "بررسی تخصصی")]
    public string SpecialtyCheck { get; set; }

    [Display(Name = "نام فارسی کالا")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string PersianTitle { get; set; }

    [Display(Name = "نام انگلیسی کالا")]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string EnglishTitle { get; set; }

    [Display(Name = "تصاویر محصول")]
    [IsImage(true)]
    [FileRequired]
    [MaxFileSize(3,true)]
    public List<IFormFile> ProductImageFiles { get; set; }
    
    
    [Display(Name = "ویدیو محصول")]
    [MaxFileSize(10,true)]
    [AllowExtensions(new[] { "mp4" }, new[] { "video/mp4" },true )]
    public List<IFormFile> ProductVideoFiles { get; set; }
}




