using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Helpers;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Product;

namespace ProShop.web.Pages.Compare;

public class IndexModel : PageBase
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IViewRenderService _viewRenderService;
    public IndexModel(IProductService productService, ICategoryService categoryService, IViewRenderService viewRenderService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _viewRenderService = viewRenderService;
    }

    [BindProperty]
    public List<ShowProductInCompareViewModel> ShowProductInCompare { get; set; } = new();

    public async Task<IActionResult> OnGet(int productCode1, int productCode2, int productCode3, int productCode4)
    {
        if (productCode1 < 1)
            return NotFound();

        if (!await _categoryService.CheckProductCategoryIdsInComparePage(productCode1, productCode2, productCode3, productCode4))
            return BadRequest();


        ShowProductInCompare = await _productService.GetProductCompare(productCode1, productCode2, productCode3, productCode4);
        return Page();
    }

    public async Task<IActionResult> OnGetShowAddProductForCompare(int[] productCodeToHide, int pageNumber = 1 , string searchValue = "")
    {
        if (!productCodeToHide.Any())
            return Json(new JsonResultOperation(false));


        var result = await _productService.GetProductsForAddProductInCompare(pageNumber,searchValue,productCodeToHide);

        if (result.PageNumber == 1)
        {
            return Json(new JsonResultOperation(true, string.Empty)
            {
                Data = new
                {
                    productBody = await _viewRenderService.RenderViewToStringAsync("~/Pages/Compare/_AddProductForCompare.cshtml", result),
                    pageNumber = result.PageNumber,
                    isLastPage=result.IsLastPage
                }

            });

        }
        else
        {
            return Json(new JsonResultOperation(true, string.Empty)
            {
                Data = new
                {
                    productBody = await _viewRenderService.RenderViewToStringAsync("~/Pages/Compare/_ProductInAddProductPartial.cshtml", result.products),
                    productCount = result.products.Count,
                    pageNumber = result.PageNumber,
                    isLastPage = result.IsLastPage

                }

            });
        }
    }


    public async Task<IActionResult> OnGetGetProductsForCompare(int productCode1, int productCode2, int productCode3, int productCode4)
    {
        if (productCode1 < 1)
            return RecordNotFound();

        if (!await _categoryService.CheckProductCategoryIdsInComparePage(productCode1, productCode2, productCode3, productCode4))
            return jsonBadRequest();


        ShowProductInCompare = await _productService.GetProductCompare(productCode1, productCode2, productCode3, productCode4);
        return Partial("_CompareBody", ShowProductInCompare);
    }
}