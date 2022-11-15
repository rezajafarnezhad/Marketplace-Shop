using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;
using ProShop.ViewModels.ProductStock;

namespace ProShop.web.Pages.Inventory.ProductStock;

[CheckModelStateInRazorPages]
public class AddProductStockByConsignmentModel : InventoryPanelBase
{
    private readonly IProductStockService _productStockService;
    private readonly IConsignmentItemService _consignmentItemService;
    private readonly IConsignmentService _consignmentService;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper _mapper;

    public AddProductStockByConsignmentModel(IUnitOfWork unitOfWork, IConsignmentItemService consignmentItemService, IProductStockService productStockService, IMapper mapper, IConsignmentService consignmentService)
    {
        this.unitOfWork = unitOfWork;
        _consignmentItemService = consignmentItemService;
        _productStockService = productStockService;
        _mapper = mapper;
        _consignmentService = consignmentService;
    }

    [BindProperty]
    public AddProductStockByConsignmentViewModel AddProductStock { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {

        if(!await _consignmentService.CanAddStockForConsignmentItems(AddProductStock.ConsignmentId))
            return Json(new JsonResultOperation(false, "موجودی این محموله قادر به افزایش و تغییر نمیباشد"));


        if (!await _consignmentItemService.IsExistsByProductVariantIdAndConsignmentId(AddProductStock.ProductVariantId, AddProductStock.ConsignmentId))
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var addOrUpdate = string.Empty;
        var AddProductStockToDatabase = await _productStockService.GetByProductVariantIdAndConsignmentId(AddProductStock.ProductVariantId, AddProductStock.ConsignmentId);
        if (AddProductStockToDatabase is null)
        {
            addOrUpdate = "افزایش";
            AddProductStockToDatabase = _mapper.Map<Entities.ProductStock>(AddProductStock);
            await _productStockService.AddAsync(AddProductStockToDatabase);
        }
        else
        {
            addOrUpdate = "ویرابش";
            AddProductStockToDatabase.Count = AddProductStock.Count;
        }
        await unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true , $"موجودی محصول مورد نظر با موفقیت {addOrUpdate} یافت"));
    }
}
