using AutoMapper;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Attributes;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.DataLayer.Migrations;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Consignments;

namespace ProShop.web.Pages.Inventory.Consignment;

[CheckModelStateInRazorPages]
public class IndexModel : InventoryPanelBase
{
    private readonly IConsignmentService _consignmentService;
    public readonly ISellerService _sellerService;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHtmlSanitizer _htmlSanitizer;
    private readonly IProductStockService _productStockService;
    private readonly IProductService _productService;
    private readonly IProductVariantService _productVariantService;
    public IndexModel(IConsignmentService consignmentService, IUnitOfWork unitOfWork, ISellerService sellerService, IMapper mapper, IHtmlSanitizer htmlSanitizer, IProductStockService productStockService, IProductVariantService productVariantService, IProductService productService)
    {
        _consignmentService = consignmentService;
        this.unitOfWork = unitOfWork;
        _sellerService = sellerService;
        _mapper = mapper;
        _htmlSanitizer = htmlSanitizer;
        _productStockService = productStockService;
        _productVariantService = productVariantService;
        _productService = productService;
    }

    [BindProperty(SupportsGet = true)]
    public ShowConsignmentsViewModel Consignment { get; set; }
    public void OnGet()
    {
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

        return Partial("_List", await _consignmentService.GetConsignments(Consignment));
    }

    public async Task<IActionResult> OnGetGetConsignmentDetails(long consignmentId)
    {
        if (consignmentId < 1)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));


        var data = await _consignmentService.GetConsignmentDetails(consignmentId);
        if (data.ConsignmentItems.Count < 1)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        return Partial("_ConsignmentDetails", data);
    }

    public async Task<IActionResult> OnGetAutocompleteSearchForShopName(string term)
    {
        var data = await _sellerService.GetShopNameForAutocomplete(term);
        return Json(data);

    }

    public async Task<IActionResult> OnPostConfirmConsignment(long consignmentId)
    {
        if (consignmentId < 1)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var consignmet = await _consignmentService.GetConsignmentForConfirmation(consignmentId);
        if (consignmet is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        consignmet.ConsignmentStatus = ConsignmentStatus.ConfirmAndAwaitingForConsignment;
        await unitOfWork.SaveChangesAsync();
        // Send email to the seller
        return Json(new JsonResultOperation(true, "محموله مورد نظر با موفقیت تایید شد و در انتظار ارسال محموله توسط فروشنده قرار گرفت"));

    }

    public async Task<IActionResult> OnPostReceiveConsigment(long consignmentId)
    {
        if (consignmentId < 1)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var consignmet = await _consignmentService.GetConsignmentToReceive(consignmentId);
        if (consignmet is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        consignmet.ConsignmentStatus = ConsignmentStatus.Received;
        await unitOfWork.SaveChangesAsync();
        // Send email to the seller
        return Json(new JsonResultOperation(true,
             "محموله مورد نظر با موفقیت دریافت شد، لطفا موجودی کالا ها را افزایش دهید"));

    }


    public async Task<IActionResult> OnGetGetChangeConsignmentStatus(long consignmentId)
    {
        if (consignmentId < 1)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));


        if (!await _consignmentService.IsExistsConsignmetWithReceivedStatus(consignmentId))
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var data = new AddDescriptionForConsignmentViewModel()
        {
            ConsignmentId = consignmentId,
        };

        return Partial("_ChangeConsignmentStatusToReceivedAndAddStock", data);
    }

    public async Task<IActionResult> OnPostChangeConsignmentStatusToReceivedAndAddStock(AddDescriptionForConsignmentViewModel model)
    {
        var consignmentToChangeStatus = await _consignmentService.GetConsignmentWithReceivedStatus(model.ConsignmentId);
        if (consignmentToChangeStatus is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));


        consignmentToChangeStatus.Description = _htmlSanitizer.Sanitize(model.Description);
        consignmentToChangeStatus.ConsignmentStatus = ConsignmentStatus.ReceivedAndAddStock;

        #region Add Stock

        var ProductStocks = await _productStockService.GetProductStocksForAddProductVariantCount(model.ConsignmentId);
        var ProductVariantIds = ProductStocks.Select(c => c.Key).ToList();
        var ProductVariants = await _productVariantService.GetProductVariantsToAddCount(ProductVariantIds);
        foreach (var item in ProductStocks)
        {
            var productVariant = ProductVariants.SingleOrDefault(c => c.Id == item.Key);
            if (productVariant is not null)
            {
                productVariant.Count += item.Value;
            }
        }
        #endregion

        var productIds = ProductVariants.Select(c => c.ProductId).Distinct().ToList();
        var productToChangeTheirStatus = await _productService.GetProductsForChangeStatus(productIds);

        foreach (var Product in productToChangeTheirStatus)
        {
            if (Product.ProductStockStatustus == Entities.ProductStockStatus.Unavailable)
                Product.ProductStockStatustus = Entities.ProductStockStatus.Available;

        }
        await unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true,
              "موجودی کالا ها باموفقیت افزایش یافت"));

    }
}
