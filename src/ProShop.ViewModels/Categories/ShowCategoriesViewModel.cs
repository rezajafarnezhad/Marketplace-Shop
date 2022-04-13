using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ProShop.Common.Constants;

namespace ProShop.ViewModels.Categories;


public class ShowCategoryViewModel
{
    [Display(Name = "عنوان")]
    public string Title { get; set; }
    [Display(Name = "سر دسته")]
    public string Parent { get; set; }
    
    [Display(Name = "آدرس دسته بندی")]
    public string slug { get; set; }

    [Display(Name = "نمایش در منوهای اصلی")]
    public bool ShowInMenus { get; set; }

    [Display(Name = "تصویر")]
    public string Picture { get; set; }
    

}

public class ShowCategoriesViewModel
{
    public List<ShowCategoryViewModel> Categories { get; set; } = new();
    public SearchCategoriesViewModel SearchCategories { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
}

public class SearchCategoriesViewModel
{
    [Display(Name = "عنوان")]
    [MaxLength(100,ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? Title { get; set; }

    [Display(Name = "آدرس دسته بندی")]
    [MaxLength(300, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]

    public string? Slug { get; set; }

    [Display(Name = "وضعیت حذف شده ها")]
    public DeletedStatus DeletedStatus { get; set; }

    [Display(Name = "نمایش در منو های اصلی")]
    public ShowInMenusStatus ShowInMenusStatus { get; set; }
}

public enum ShowInMenusStatus
{
    [Display(Name = "همه")]
    All,

    [Display(Name = "بله")]

    True,
    [Display(Name = "خیر")]

    False,
}