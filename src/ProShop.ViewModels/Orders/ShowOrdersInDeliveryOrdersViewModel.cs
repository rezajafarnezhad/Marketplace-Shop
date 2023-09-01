using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using ProShop.Entities;

namespace ProShop.ViewModels.Orders;

public class ShowOrdersInDeliveryOrdersViewModel
{
    public PaginationViewModel Pagination { get; set; } = new();
    public List<ShowOrderInDeliveryOrderViewModel> Orders { get; set; } = new();
    public SearchOrderInDeliveryOrdersViewModel SearchOrders { get; set; } = new();
}

public class ShowOrderInDeliveryOrderViewModel
{

    [Display(Name = "شناسه")]
    public long Id { get; set; }

    [Display(Name = "گیرنده")]

    public string AddressFullName { get; set; }

    [Display(Name = "مقصد")]
    public string Destination { get; set; }

    [Display(Name = "وضعیت")]

    public OrderStatus OrderStatus { get; set; }

    [Display(Name = "شماره سفارش")]
    public long OrderNumber { get; set; }

    [Display(Name = "تاریخ ایجاد")]
    public string CreatedDateTime { get; set; }

    public List<ShowParcelPostInDeliveryOrdersViewModel> ParcalPosts { get; set; } = new();
    public int ParcalPostsCount { get; set; }
    public int ParcelPostsCountInPost { get; set; }
}

public class ShowParcelPostInDeliveryOrdersViewModel
{
    public long Id { get; set; }
    public ProductDimensions Dimensions { get; set; }
    public ParcelPostStatus ParcelPostStatus { get; set; }

}

    public class SearchOrderInDeliveryOrdersViewModel
{
    [EqualSearch]
    [Range(1, long.MaxValue, ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    [Display(Name = "شماره سفارش")]
    public long? OrderNumber { get; set; }

    [Display(Name = "گیرنده")]
    [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
        ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
    [MaxLength(400, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string FullName { get; set; }

    [Display(Name = "تاریخ ایجاد")]
    [RegularExpression(@"^[۰-۹]{4}\/(۰[۱-۹]|۱[۰-۲])\/(۰[۱-۹]|[۱۲][۰-۹]|۳[۰۱])$", ErrorMessage = "تاریخ به درستی وارد شود")]
    [EqualDateTimeSearch]

    public string CreatedDateTime { get; set; }

    [Display(Name = "نمایش بر اساس")]
    public SortingOrders Sorting { get; set; }

    [Display(Name = "مرتب سازی بر اساس")]
    public SortingOrder SortingOrder { get; set; } = SortingOrder.Desc;


    [Display(Name = "استان")]
    [Range(1, long.MaxValue, ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public int? ProvinceId { get; set; }
    public List<SelectListItem> Provinces { get; set; } = new();

    [Display(Name = "شهرستان")]
    // [Range(1, long.MaxValue, ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public int? CityId { get; set; }

    [Display(Name = "وضعیت")]
    [EqualEnumSearch]
    public OrderStatusInDeliveryOrders? OrderStatus { get; set; }
}

public enum OrderStatusInDeliveryOrders : byte
{
    
    [Display(Name = "پردازش انبار")]
    InventoryProcessing=2,

    [Display(Name = "بخشی از مرسوله ها در پست")]
    SomeParcelsDeliveredToPost=3,
    [Display(Name = "تمام مرسوله ها در پست")]

    CompletelyParcelsDeliveredToPost=4,
    [Display(Name = "تحویل شده")]
    DeliveredToClient=5,
}
