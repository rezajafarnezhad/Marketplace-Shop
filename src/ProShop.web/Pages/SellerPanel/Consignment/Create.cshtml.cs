using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Consignments;
using ProShop.ViewModels.ProductVariant;
using System.ComponentModel.DataAnnotations;

namespace ProShop.web.Pages.SellerPanel.Consignment;

public class CreateModel : SellerPanelBase
{

    private readonly IProductVariantService _productVariantService;
    private readonly IViewRenderService _viewRenderService;
    private readonly ISellerService _sellerService;
    private readonly IUnitOfWork unitOfWork;
    private readonly IConsignmentService _consignmentService;
    public CreateModel(IProductVariantService productVariantService, IViewRenderService viewRenderService, ISellerService sellerService, IUnitOfWork unitOfWork, IConsignmentService consignmentService)
    {
        _productVariantService = productVariantService;
        _viewRenderService = viewRenderService;
        _sellerService = sellerService;
        this.unitOfWork = unitOfWork;
        _consignmentService = consignmentService;
    }

    
    [Display(Name = "کد تنوع محصول")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Range(1, int.MaxValue, ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    public int VariantCode { get; set; }


    public CreateConsignmentViewModel CreateConsignment { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(CreateConsignmentViewModel CreateConsignment)
    {
        if (!ModelState.IsValid)
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });

        if (CreateConsignment.Variants.Count < 1)
            return Json(new JsonResultOperation(false));


        var deliveryDate = CreateConsignment.DeliveryDate.ToGregorianDateTime();
        if (!deliveryDate.Issuccessful)
            return Json(new JsonResultOperation(false));

        //2|||44

        var variantCodes = new List<int>();
        foreach (var VariantCode in CreateConsignment.Variants)
        {
            var splitVariant = VariantCode.Split("|||");
            if (!int.TryParse(splitVariant[0], out var variantCodeToAdd))//2
            {
                return Json(new JsonResultOperation(false));
            }
            variantCodes.Add(variantCodeToAdd);
        }

        if (CreateConsignment.Variants.Count != CreateConsignment.Variants.Distinct().Count())
        {
            return Json(new JsonResultOperation(false));

        }

        var consignmentToAdd = new Entities.Consignment()
        {
            DeliveryDate = deliveryDate.result,
            sellerId = await _sellerService.GetSellerId(),
        };

        var productVariants = await _productVariantService.GetProductVariantsForCreateConsignmet(variantCodes);
        if (productVariants.Count != variantCodes.Count)
            return Json(new JsonResultOperation(false));

        foreach (var productVariant in productVariants)
        {
            var variantCodeToCompare = $"{productVariant.VariantCode}|||";
            var variantItem = CreateConsignment.Variants.Single(c => c.StartsWith(variantCodeToCompare));
            var productCountString = variantItem.Split("|||")[1];
            if (!int.TryParse(productCountString, out var productCount))//44
            {
                return Json(new JsonResultOperation(false));
            }

            var maxProductCount = 100000;
            if(productCount>maxProductCount)
                return Json(new JsonResultOperation(false));

            consignmentToAdd.ConsignmentItems.Add(new Entities.ConsignmentItem()
            {
                Count = productCount,
                ProductVariantId= productVariant.Id,
                Barcode=$"{productVariant.Id}--{consignmentToAdd.sellerId}"
            });
        }
        await _consignmentService.AddAsync(consignmentToAdd);
        await unitOfWork.SaveChangesAsync();
       
        return Json(new JsonResultOperation(true,"محموله مورد نظر ایجاد شد"));
    }

    public async Task<IActionResult> OnPostGetConsignmentTr(int VariantCode)
    {
        if (!ModelState.IsValid)
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });

        var data = await _productVariantService.GetProductVariantForCreateConsignmet(VariantCode);
        if (data is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));


        return Json(new JsonResultOperation(true, String.Empty)
        {

            Data = await _viewRenderService.RenderViewToStringAsync("~/Pages/SellerPanel/Consignment/_productVariantTrPar.cshtml", data)
        });
    }
}
