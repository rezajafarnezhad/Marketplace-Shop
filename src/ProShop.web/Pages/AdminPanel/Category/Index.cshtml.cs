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

            categoriesViewModel.Pagination.Take = 6;
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
            await _uploadFileService.SaveFile(model.Picture, picturefileName, null, "images", "Categories");
            return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت ذخیره شد"));
        }



        public async Task<IActionResult> OnGetEdit(long Id)
        {
            var model = await _categoryService.GetForEdit(Id);
            if (model is null)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }
            model.MainCategories = _categoryService.GetCategoriesToShowInSelectBox()
                .CreateSelectListItem(firstItemText: "خودش سر دسته اصلی است");
            return Partial("Edit", model);
        }

        public async Task<IActionResult> OnPostEdit(EditCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
                {
                    Data = ModelState.GetModelStateErrors()
                });
            }



            var _category = await _categoryService.FindByIdAsync(model.Id);
            if (_category is null)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }


            if (model.Picture.IsFileUploaded())
            {
                string picturefileName = null;
                var oldFileName = _category.Picture;
                picturefileName = model.Picture.GenerateFileName();
                _category.Picture = picturefileName;
                await _uploadFileService.SaveFile(model.Picture, picturefileName, oldFileName, "images", "Categories");
            }
            _category.Title = model.Title;
            _category.Slug = model.Slug;
            _category.ParentId = model.ParentId is 0 ? null : model.ParentId;
            _category.Description = model.Description;
            _category.IsShowInMenus = model.IsShowInMenus;


            var result = await _categoryService.Update(_category);

            if (!result.Ok)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.DuplicateErrorMessage)
                {
                    Data = result.Columns.SetDuplicateColumnsErrors<EditCategoryViewModel>()
                });
            }
            await _unitOfWork.SaveChangesAsync();
            return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت ویرایش شد"));
        }


        public async Task<IActionResult> OnPostDelete(long elementId)
        {
            var _category = await _categoryService.FindByIdAsync(elementId);
            if (_category is null)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }
            await _categoryService.SoftDelete(_category);
            await _unitOfWork.SaveChangesAsync();
            return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت حذف شد"));
        }
    }
}
