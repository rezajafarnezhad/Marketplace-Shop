using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Categories;

namespace ProShop.web.Pages.AdminPanel.Category
{
    public class IndexModel : PageBase
    {

        #region Ctor

        private readonly ICategoryService _categoryService;
        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        #endregion


        public ShowCategoriesViewModel ShowCategoriesViewModel { get; set; } = new();
        public SearchCategoriesViewModel SearchCategories { get; set; } = new();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetGetDataTableAsync(SearchCategoriesViewModel searchCategories)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
                {
                    Data = ModelState.GetModelStateErrors()
                });
            }

            return Partial("_List", await _categoryService.GetCategories(searchCategories));
        }
    }
}
