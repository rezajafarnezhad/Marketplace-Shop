using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Orders;
using ProShop.ViewModels.ParcelPosts;

namespace ProShop.web.Pages.Inventory.DeliveryOrder;


[CheckModelStateInRazorPages]
public class IndexModel : InventoryPanelBase
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProvinceAndCityService _provinceAndCityService;
    private readonly IParcelPostService _parcelPostService;
    public IndexModel(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork, IProvinceAndCityService provinceAndCityService, IParcelPostService parcelPostService)
    {
        _orderService = orderService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _provinceAndCityService = provinceAndCityService;
        _parcelPostService = parcelPostService;
    }


    [BindProperty(SupportsGet = true)]
    public ShowOrdersInDeliveryOrdersViewModel ShowOrders { get; set; } = new();

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

        return Partial("_List", await _orderService.GetDeliveryOrders(ShowOrders));
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

    public async Task<IActionResult> OnGetOrderDetails(long orderId)
    {
        if (orderId < 0)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var model = await _orderService.GetOrderDetails(orderId);
        if (model is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        return Partial("../Inventory/Order/_OrderDetails", model);
    }


    public async Task<IActionResult> OnGetShowDeliveryToPost(long Id)
    {
        if (!await _parcelPostService.IsExistsBy(nameof(Entities.ParcalPost.Id), Id))
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        }

        return Partial("_DeliveryToPost"); ;
    }

    public async Task<IActionResult> OnPostChangeStatusToDeliveryToPost(DeliveryParcelPostViewModel model)
    {
        var _parcelPost = await _parcelPostService.FindAsync(model.Id);
        if(_parcelPost is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        _parcelPost.PostTrackingCode = model.PostTrackingCode;  
        _parcelPost.ParcelPostStatus = ParcelPostStatus.DeliveredToPost;
        await _unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "وضعیت مرسوله مورد نظر به \"تحویل داده شده به اداره پست\" تغییر یافت"));

    }
}