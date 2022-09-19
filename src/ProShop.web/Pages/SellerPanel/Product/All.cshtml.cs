using AutoMapper;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using ProShop.Common;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Product;


namespace ProShop.web.Pages.SellerPanel.Product;

public class AllModel : SellerPanelBase
{

    private readonly IProductService _productService;
    private readonly ISellerService _sellerService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUploadFileService _uploadFileService;
    private readonly IHtmlSanitizer _htmlSanitizer;
    public AllModel(IProductService productService, IMapper mapper, ISellerService sellerService, ICategoryService categoryService, IUnitOfWork unitOfWork, IUploadFileService uploadFileService, IHtmlSanitizer htmlSanitizer)
    {
        _productService = productService;
        _mapper = mapper;
        _sellerService = sellerService;
        _categoryService = categoryService;
        _unitOfWork = unitOfWork;
        _uploadFileService = uploadFileService;
        _htmlSanitizer = htmlSanitizer;
    }

    [BindProperty(SupportsGet = true)]
    public ShowAllProductsInSellerPanelViewModel Products { get; set; } = new();

    public void OnGet()
    {
        var categories = _categoryService.GetCategoriesWithNoChild().Result;
        Products.SearchProducts.Categories = categories.CreateSelectListItem(firstItemText: "همه", firstItemValue: "");
    }

    public async Task<IActionResult> OnGetGetDataTableAsync()
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {

                Data = ModelState.GetModelStateErrors()
            });
        }

        return Partial("_ListAll", await _productService.GetAllProductsInSellerPanel(Products));
    }

    public async Task<IActionResult> OnGetAutocompleteSearchForPersianTitle(string term)
    {
        return Json(await _productService.GetPersianTitlesForAutocomplete(term));
    }

    public async Task<IActionResult> OnGetGetProductDetails(long productId)
    {
        var data = await _productService.GetProductDetails(productId);
        if (data is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        return Partial("_ProductDetails", data);

    }

   

 
}
