using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Helpers;
using ProShop.Services.Contracts;

namespace ProShop.web.Pages.SellerPanel.Product;

public class CreateModel : SellerPanelBase
{

    private readonly ICategoryService _categoryService;

    public CreateModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> OnGetGetCategories(long[]? selectedCategoriesIds)
    {
        var result = await _categoryService.GetCategoriesForCreateProduct(selectedCategoriesIds);
        return Partial("_SelectProductCategoryPartial", result);

    }
}