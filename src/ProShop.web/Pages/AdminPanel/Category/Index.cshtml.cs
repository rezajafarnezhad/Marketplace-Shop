using Microsoft.AspNetCore.Mvc;
using ProShop.Common;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Categories;

namespace ProShop.web.Pages.AdminPanel.Category
{
    public class IndexModel : PageBase
    {

        #region Ctor

        private readonly ICategoryService _categoryService;
        private readonly IUploadFileService _uploadFileService;
        private readonly IUnitOfWork _unitOfWork;
        public IndexModel(ICategoryService categoryService, IUnitOfWork unitOfWork, IUploadFileService uploadFileService)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
            _uploadFileService = uploadFileService;
        }

        #endregion

        public ShowCategoriesViewModel CategoriesViewModel { get; set; } = new();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetGetDataTableAsync(ShowCategoriesViewModel categoriesViewModel)
        {

            if (!ModelState.IsValid)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
                {
                    Data = ModelState.GetModelStateErrors()
                });
            }

            categoriesViewModel.Pagination.Take = 1;
            return Partial("_List", await _categoryService.GetCategories(categoriesViewModel));
        }


        public IActionResult OnGetAdd()
        {
            var model = new AddCategoryViewModel()
            {
                MainCategories = _categoryService.GetCategoriesToShowInSelectBox()
                    .CreateSelectListItem(firstItemText: "خودش سر دسته اصلی است")

            };
            return Partial("Add", model);
        }

        public async Task<IActionResult> OnPostAdd(AddCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
                {
                    Data = ModelState.GetModelStateErrors()
                });
            }

            string picturefileName = null;
            if (model.Picture.IsFileUploaded())
            {
                picturefileName = model.Picture.GenerateFileName();
            }

            var _category = new Entities.Category()
            {
                Title = model.Title,
                Slug = model.Slug,
                Description = model.Description,
                IsShowInMenus = model.IsShowInMenus,
                ParentId = model.ParentId is 0 ? null : model.ParentId,
                IsDeleted = false,
                Picture = picturefileName
            };

            var result = await _categoryService.AddAsync(_category);
            if (!result.Ok)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.DuplicateErrorMessage)
                {
                    Data = result.Columns.SetDuplicateColumnsErrors<AddCategoryViewModel>()
                });
            }
            
            await _unitOfWork.SaveChangesAsync();
            await _uploadFileService.SaveFile(model.Picture, picturefileName, "images","Categories");
            return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت ذخیره شد"));
        }

    }
}
