using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.Services.Implements;
using ProShop.ViewModels.Product;

namespace ProShop.web.Pages.SellerPanel.Product
{
    public class IndexModel : SellerPanelBase
    {

        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public IndexModel(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
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
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
                {

                    Data = ModelState.GetModelStateErrors()
                });
            }

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
    }

}
