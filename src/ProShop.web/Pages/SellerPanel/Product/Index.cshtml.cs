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
using ProShop.Services.Implements;
using ProShop.ViewModels.Product;
using ProShop.ViewModels.ProductVariant;

namespace ProShop.web.Pages.SellerPanel.Product;


public class IndexModel : SellerPanelBase
{

    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private readonly IProductVariantService _productVariantService;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper _mapper;
    public IndexModel(ICategoryService categoryService, IProductService productService, IProductVariantService productVariantService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _categoryService = categoryService;
        _productService = productService;
        _productVariantService = productVariantService;
        this.unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [BindProperty(SupportsGet = true)]
    public ShowProductsInSellerPanelViewModel Products { get; set; } = new();
    public  void OnGet()
    {
        var categories =  _categoryService.GetSellerCategories().Result;
        Products.SearchProducts.Categories = categories.CreateSelectListItem(firstItemText: "همه", firstItemValue: "");
    }
    public async Task<IActionResult> OnGetGetDataTableAsync()
    {
      

        return Partial("_List", await _productService.GetProductsInSellerPanel(Products));
    }


    public async Task<IActionResult> OnGetAutocompleteSearchForPersianTitle(string term)
    {
        return Json(await _productService.GetPersianTitlesForAutocompleteInSellerPanel(term));
    }
    public async Task<IActionResult> OnGetGetProductDetails(long productId)
    {
        var data = await _productService.GetProductDetails(productId);
        if (data is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        return Partial("_ProductDetails", data);

    }

    public async Task<IActionResult> OnGetGetProductVariants(long productId)
    {
        var data = await _productVariantService.GetProductVariants(productId);
        if(data is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        return Partial("_ProductVariants", data);
    }


    public async Task<IActionResult> OnGetEditProductVariant(long ProdcuctVariantId)
    {
        if (ProdcuctVariantId < 1)
            return Json(new JsonResultOperation(false));

        var productVariant = await _productVariantService.GetDateForEdit(ProdcuctVariantId);
        if(productVariant is null)
            return Json(new JsonResultOperation(false,PublicConstantStrings.RecordNotFoundErrorMessage));

        return Partial("EditProductVariant", productVariant);
    } 
    
    public async Task<IActionResult> OnPostEditProductVariant(EditProductVariantViewModel model)
    {
        var productVariant = await _productVariantService.GetforEdit(model.Id);
        if(productVariant is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));


        productVariant = _mapper.Map(model, productVariant);
        productVariant.StartDateTime = productVariant.EndDateTime = null;
        productVariant.OffPrice = productVariant.offPercentage = null;

         _mapper.Map<Entities.ProductVariant>(model);

        await unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "تنوع محصول با موفقیت ویرایش شد"));

    }




    public async Task<IActionResult> OnGetAddEditDiscount(long ProdcuctVariantId)
    {
        if (ProdcuctVariantId < 1)
            return Json(new JsonResultOperation(false));

        var productVariant = await _productVariantService.GetDateForAddEditDiscount(ProdcuctVariantId);
        if (productVariant is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        return Partial("AddEditDiscount", productVariant);
    }

    public async Task<IActionResult> OnPostAddEditDiscount(AddEditDiscountViewModel model)
    {

        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        var productVariant = await _productVariantService.GetforEdit(model.Id);


        if (productVariant is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        
        var parsedDateTimes = DateTimeHelper.ConvertDateTimeForAddEditDiscount(model.StartDateTime, model.EndDateTime);
        if (!parsedDateTimes.IsSuccessful)
        {
            return Json(new JsonResultOperation(false, "لطفا تاریخ ها را به درستی وارد نمایید"));
        }

        if (parsedDateTimes.IsStartDateTimeGreatherOrEqualEndDateTime)
        {
            return Json(new JsonResultOperation(false, "تاریخ پایان تخفیف باید بزرگتر از تاریخ شروع تخفیف باشد"));
        }

        if (parsedDateTimes.IsTimeSpanLowerThan3Hour)
        {
            return Json(new JsonResultOperation(false, "تاریخ پایان تخفیف باید حداقل 3 ساعت بزرگتر از تاریخ شروع تخفیف باشد"));
        }

        productVariant.StartDateTime = parsedDateTimes.StartDate;
        productVariant.EndDateTime = parsedDateTimes.EndDate;
        _mapper.Map(model, productVariant);
        await unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "نخفیف ثبت شد"));

    }
}

