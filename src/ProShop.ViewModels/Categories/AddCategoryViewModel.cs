using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;

namespace ProShop.ViewModels.Categories;

public class AddCategoryViewModel
{


    [PageRemote(PageName = "Index", PageHandler = "CheckForTitle",
        HttpMethod = "POST",
        ErrorMessage = AttributesErrorMessages.RemoteMessage,
        AdditionalFields = ViewModelConstants.AntiForgeryToken)]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [Display(Name = "عنوان")]
    public string Title { get; set; }
    [Display(Name = "توضیحات")]

    public string? Description { get; set; }

    [PageRemote(PageName = "Index", PageHandler = "CheckForSlug",
        HttpMethod = "POST",
        ErrorMessage = AttributesErrorMessages.RemoteMessage,
        AdditionalFields = ViewModelConstants.AntiForgeryToken)]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(300, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [Display(Name = "آدرس دسته بندی")]

    public string Slug { get; set; }
   
    [Display(Name = "تصویر")]
    [MaxFileSize(2)]
    [IsImage]
    public IFormFile Picture { get; set; }

    [Display(Name = "نمایش در منو های اصلی")] 
    public bool IsShowInMenus { get; set; }

    [Display(Name = "آیا میتوان کالای غیر اصل اضافه کرد ؟")]
    public bool CanAddFakeProduct { get; set; }
    
    [Display(Name = "سردسته")]

    public long? ParentId { get; set; }

    public List<SelectListItem> MainCategories { get; set; } = new();

}