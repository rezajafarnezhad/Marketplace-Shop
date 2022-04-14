using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;

namespace ProShop.ViewModels.Categories;

public class EditCategoryViewModel : AddCategoryViewModel
{
    public long Id { get; set; }

    [Display(Name = "تصویر انتخاب شده")]
    public string? SelectedPicture { get; set; }
}