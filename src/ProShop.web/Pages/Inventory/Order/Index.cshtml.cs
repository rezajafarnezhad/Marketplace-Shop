using AutoMapper;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.Services.Implements;
using ProShop.ViewModels.Brands;
using ProShop.ViewModels.Orders;

namespace ProShop.web.Pages.Inventory.Order;

public class IndexModel : InventoryPanelBase
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProvinceAndCityService _provinceAndCityService;
    public IndexModel(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork, IProvinceAndCityService provinceAndCityService)
    {
        _orderService = orderService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _provinceAndCityService = provinceAndCityService;
    }


    [BindProperty(SupportsGet = true)]
    public ShowOrdersViewModel ShowOrders { get; set; } = new();

    public async void OnGet()
    {
        var _Provinces = await _provinceAndCityService.GetProvincesToShowSelectBox();
        ShowOrders.SearchOrders.Provinces = _Provinces.CreateSelectListItem(firstItemValue: String.Empty);
    }

    public async Task<IActionResult> OnGetGetDataTableAsync()
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(String.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        return Partial("_List", await _orderService.GetOrders(ShowOrders));
    }
    public async Task<IActionResult> OnGetGetCities(long provinceId)
    {
        if (provinceId == 0)
            return Json(new JsonResultOperation(true, string.Empty)
            {
                Data = new Dictionary<long, string>()
            });


        if (provinceId < 1)
            return Json(new JsonResultOperation(false, "شهرستان مورد نظر یافت نشد"));


        if (!await _provinceAndCityService.IsExistsBy(nameof(Entities.ProvinceAndCity.Id), provinceId))
            return Json(new JsonResultOperation(false, "شهرستان مورد نظر یافت نشد"));


        var _cities = await _provinceAndCityService.GetCitiesByProvinceIdInSelectBox(provinceId);
        return Json(new JsonResultOperation(true, string.Empty)
        {
            Data = _cities
        });
    }


    public async Task<IActionResult> OnPostChangeStatusToInventoryProcessing(long OrderId)
    {
        var _order = await _orderService.FindByIdWithIncludesAsync(OrderId,nameof(Entities.Order.ParcalPosts));
        if (_order is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        _order.OrderStatus = OrderStatus.InventoryProcessing;

        foreach (var parcelPost in _order.ParcalPosts)
        {
            parcelPost.ParcelPostStatus = ParcelPostStatus.InventoryProcessing;
        }
        await _unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "سفارش مورد نظر وارد مرحله پردازش انبار شد"));
    }

    public async Task<IActionResult> OnGetOrderDetails(long orderId)
    {
        if (orderId < 0)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var model = await _orderService.GetOrderDetails(orderId);
        if(model is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        return Partial("_OrderDetails", model);
    }
}