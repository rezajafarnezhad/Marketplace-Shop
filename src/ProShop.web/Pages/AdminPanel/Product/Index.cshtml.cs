using AngleSharp.Dom;
using AutoMapper;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using ProShop.Common;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Product;

namespace ProShop.web.Pages.AdminPanel.Product;

public class IndexModel : PageBase
{

    private readonly IProductService _productService;
    private readonly ISellerService _sellerService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUploadFileService _uploadFileService;
    private readonly IHtmlSanitizer _htmlSanitizer;
    private readonly IProductShortLinkService _productShortLinkService;
    public IndexModel(IProductService productService, IMapper mapper, ISellerService sellerService, ICategoryService categoryService, IUnitOfWork unitOfWork, IUploadFileService uploadFileService, IHtmlSanitizer htmlSanitizer, IProductShortLinkService productShortLinkService)
    {
        _productService = productService;
        _mapper = mapper;
        _sellerService = sellerService;
        _categoryService = categoryService;
        _unitOfWork = unitOfWork;
        _uploadFileService = uploadFileService;
        _htmlSanitizer = htmlSanitizer;
        _productShortLinkService = productShortLinkService;
    }

    [BindProperty(SupportsGet =true)]
    public ShowProductsViewModel Products { get; set; } = new();

    public void OnGet()
    {
        var categories = _categoryService.GetCategoriesWithNoChild().Result;
        Products.SearchProducts.Categories = categories.CreateSelectListItem(firstItemText:"همه",firstItemValue:"");
    }

    public async Task<IActionResult> OnGetGetDataTableAsync()
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage) { 
            
                Data = ModelState.GetModelStateErrors()
            });
        }

        return Partial("_List" , await _productService.GetProducts(Products));
    }


    public async Task<IActionResult> OnGetGetProductDetails(long productId)
    {
        var data = await _productService.GetProductDetails(productId);
        if (data is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        return Partial("_ProductDetails", data);

    }

    public async Task<IActionResult> OnGetAutocompleteSearchForPersianTitle(string term)
    {
        return Json(await _productService.GetPersianTitlesForAutocomplete(term));
    }
    
    public async Task<IActionResult> OnGetAutocompleteSearchForShopName(string term)
    {
        return Json(await _sellerService.GetShopNameForAutocomplete(term));
    }

    public async Task<IActionResult> OnPostRemoveProduct(long Id)
    {
        if (Id < 1)
            return Json(new JsonResultOperation(false));

        var product = await _productService.GetProductToRemoveInManagingProducts(Id);
        if(product is null)
            return Json(new JsonResultOperation(false,"محصولی یافت نشد"));

        var shortLink = await _productShortLinkService.FindByIdAsync(product.ProductShortLinkId);
        shortLink.IsUsed = false;
        _productService.Remove(product);
        
        await _unitOfWork.SaveChangesAsync();
        foreach (var item in product.ProductMedia)
        {
            _uploadFileService.DeleteFile(item.FileName, item.IsVideo ? "Videos":"Images", "Products");
        }
        return Json(new JsonResultOperation(true, "محصول مورد نظر حذف شد"));

    }
    
    public async Task<IActionResult> OnPostConfirmProduct(long Id , ProductDimensions Dimensions)
    {
        if (Id < 1)
            return Json(new JsonResultOperation(false));

        var product = await _productService.FindByIdAsync(Id);
        if (product is null)
            return Json(new JsonResultOperation(false,"محصولی یافت نشد"));

        product.Status = Entities.ProductStatus.Confirmed;
        product.Dimensions = Dimensions;
        product.RejectReason =null;
        await _unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "محصول مورد نظر تایید شد"));

    }
    
    public async Task<IActionResult> OnPostRejectProduct(ProductDetailsViewModel model)
    {
        if (!ModelState.IsValid)
            return Json(new JsonResultOperation(false,"دلایل رد مدارک فروشنده را وارد کنید"));

        var product = await _productService.FindByIdAsync(model.Id);
        if(product is null)
            return Json(new JsonResultOperation(false,"محصولی یافت نشد"));

        product.Status = Entities.ProductStatus.Rejected;
        product.RejectReason = _htmlSanitizer.Sanitize(model.RejectReason);
        await _unitOfWork.SaveChangesAsync();
       
        return Json(new JsonResultOperation(true, "محصول مورد نظر رد شد"));

    }


}
