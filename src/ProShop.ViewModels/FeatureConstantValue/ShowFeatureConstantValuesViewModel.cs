using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using ProShop.Entities;
using ProShop.ViewModels.Brands;

namespace ProShop.ViewModels.FeatureConstantValue;

public  class ShowFeatureConstantValuesViewModel
{
    public List<ShowFeatureConstantValueViewModel> ShowFeatureConstantValues { get; set; } = new();
    public SearchFeatureConstantValueViewModel SearchFeatureConstant { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
}


public class ShowFeatureConstantValueViewModel
{
    [Display(Name = "شناسه")]
    public long Id { get; set; }

    [Display(Name = "عنوان ویژگی")]
    public string FeatureTitle { get; set; }

    [Display(Name = "عنوان دسته بندی")]
    public string CategoryTitle { get; set; }

    [Display(Name = "مقدار")]
    public string Value { get; set; }
}
public class SearchFeatureConstantValueViewModel
{
    [ContainsSearch]
    [Display(Name = "مقدار")]
    [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Value { get; set; }

    [EqualSearch]
    [Display(Name = "دسته بندی")]
    public long CategoryId { get; set; }

    public List<SelectListItem> Categories { get; set; } = new();

    [Display(Name = "ویژگی ها")]
    public long FeatureId { get; set; }

    [Display(Name = "نمایش بر اساس")]
    public SortingFeatureConstantValues Sorting { get; set; }

    [Display(Name = "مرتب سازی بر اساس")]
    public SortingOrder SortingOrder { get; set; }
}


public enum SortingFeatureConstantValues
{
    [Display(Name = "شناسه")]
    Id,

    [Display(Name = "مقدار")]

    Value
}