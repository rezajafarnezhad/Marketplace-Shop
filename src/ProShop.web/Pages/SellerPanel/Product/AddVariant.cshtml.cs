using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.DataLayer.Migrations;
using ProShop.Services.Contracts;
using ProShop.ViewModels;
using ProShop.ViewModels.Veriants;

namespace ProShop.web.Pages.SellerPanel.Product;

public class AddVariantModel : SellerPanelBase
{

    private readonly IProductService _productService;
    private readonly IVariantService _VariantService;
    private readonly ISellerService _sellerService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork unitOfWork;
    private readonly IProductVariantService _productVariantService;
    private readonly IGaranteeService _garanteeService;

    public AddVariantModel(IProductService productService, ISellerService sellerService = null, IMapper mapper = null,
        IProductVariantService productVariantService = null, IUnitOfWork unitOfWork = null,
        IGaranteeService garanteeService = null, IVariantService variantService = null)
    {
        _productService = productService;
        _sellerService = sellerService;
        _mapper = mapper;
        _productVariantService = productVariantService;
        this.unitOfWork = unitOfWork;
        _garanteeService = garanteeService;
        _VariantService = variantService;
    }

    [BindProperty] public AddVariantViewModel Variant { get; set; } = new();

    public async Task<IActionResult> OnGet(long ProductId)
    {

        var data = await _productService.GetProductInfoForAddVeriant(ProductId);
        if (data is null)
            return RedirectToPage(PublicConstantStrings.Error404PageName);

        //var Garantees = await _garanteeService.GetGaranteesForAddProductVaraint();
        //data.Garantees = Garantees.CreateSelectListItem();
        Variant = data;
        return Page();
    }


    public async Task<IActionResult> OnPost()
    {

        if (!ModelState.IsValid)
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()

            });


        var userId = User.Identity.GetLoggedUserId();
        var sellerId = await _sellerService.GetSellerId(userId);

        var checkInputs =
            await _VariantService.checkProductAndVariantTypeForAddVariant(Variant.ProductId, Variant.VariantId);

        if (!checkInputs.result)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
        }

        var ProductVaraintToAdd = _mapper.Map<Entities.ProductVariant>(Variant);
        if (checkInputs.isVariantNull)
        {
            ProductVaraintToAdd.VariantId = null;

        }

        if (await _productVariantService.isThisVariantAddedForSeller(ProductVaraintToAdd.VariantId, ProductVaraintToAdd.ProductId))
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        }

        ProductVaraintToAdd.SellerId = sellerId;
        ProductVaraintToAdd.VariantCode = await _productVariantService.GetVariantCodeForCreateProductVariant();

        if (await _productVariantService.existsProductVariant(ProductVaraintToAdd.ProductId,
                ProductVaraintToAdd.GaranteeId, ProductVaraintToAdd.VariantId, sellerId))
        {
            return Json(new JsonResultOperation(false, "این مشخصات قبلا برای این محصول ثبت شده"));
        }

        await _productVariantService.AddAsync(ProductVaraintToAdd);


        var product =
            await _productService.FindByIdWithIncludesAsync(Variant.ProductId, nameof(Entities.Product.Category));
        if (product.ProductStockStatustus == Entities.ProductStockStatus.New)
            product.ProductStockStatustus = Entities.ProductStockStatus.Unavailable;


        product.Category.HasVariant = true;

        await unitOfWork.SaveChangesAsync();


        return Json(new JsonResultOperation(true, "تنوغ محصول با موفقیت اضافه شد"));

    }

    public IActionResult OnGetGetGarantees(string term)
    {
        var result = _garanteeService.SearchOnGatanteesForSelect2Ajax(term);
        var specificGuarantee = result.Select((value, index) => new { value, index })
            .SingleOrDefault(p => p.value.Text.Contains("0 ماهه"));

        if (specificGuarantee != null)
        {
            result[specificGuarantee.index] = new ShowSelect2DataByAjaxViewModel
            {
                Text = "گارانتی اصالت و سلامت فیزیکی کالا",
                Id = specificGuarantee.value.Id
            };
        }

        return Json(new
        {
            results = result
        });
    }

}
