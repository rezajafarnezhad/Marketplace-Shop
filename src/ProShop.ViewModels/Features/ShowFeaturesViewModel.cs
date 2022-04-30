using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProShop.Common.Constants;

namespace ProShop.ViewModels.Features;

public class ShowFeaturesViewModel
{

    public List<ShowFeatureViewModel> FeatureViewModels { get; set; } = new();
    public SearchFeatureViewModel SearchFeature { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
}


public class SearchFeatureViewModel
{
    [Display(Name = "دسته بندی")]
    public long CategoryId { get; set; }

    [Display(Name = "عنوان")]
    [MaxLength(150,ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string? Title { get; set; }

    public List<SelectListItem> Categories { get; set; } = new();

    [Display(Name = "نمایش بر اساس")]
    public SortingFeatures Sorting { get; set; }

    [Display(Name = "مرتب سازی بر اساس")]
    public SortingOrder SortingOrder { get; set; }
}

public class ShowFeatureViewModel
{

    [Display(Name = "شناسه")]
    public long FeatureId { get; set; }

    [Display(Name = "عنوان")]
    public string Title { get; set; }

    public long CategoryId { get; set; }
    
}

public enum SortingFeatures
{
    [Display(Name = "شناسه")]
    Id,

    [Display(Name = "عنوان")]
    Title
}