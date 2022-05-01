using Microsoft.AspNetCore.Mvc;
using ProShop.Common;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
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
        private readonly IBrandService _brandService;
        public IndexModel(ICategoryService categoryService, IUnitOfWork unitOfWork, IUploadFileService uploadFileService, IBrandService brandService)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
            _uploadFileService = uploadFileService;
            _brandService = brandService;
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
                ModelState.AddModelError(String.Empty, PublicConstantStrings.ModelStateErrorMessage);
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
                {
                    Data = ModelState.GetModelStateErrors()
                });
            }

            return Partial("_List", await _categoryService.GetCategories(categoriesViewModel));
        }


        public async Task<IActionResult> OnGetAdd(long id = 0)
        {

            if (id > 0)
            {
                if (!await _categoryService.IsExistsBy(nameof(Entities.Category.Id), id))
                    return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }

            var model = new AddCategoryViewModel()
            {
                ParentId = id,
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
            model.MainCategories = _categoryService.GetCategoriesToShowInSelectBox(Id)
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

            if (model.Id == model.ParentId)
            {
                return Json(new JsonResultOperation(false, "یک رکورد نمیتواند والد خودش باشد"));
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

        public async Task<IActionResult> OnGetAddBrand(long categoryId)
        {
            var model = new AddBrandCategoryViewModel()
            {
                CategoryId = categoryId,
                Brands = await _categoryService.GetCategoryBrands(categoryId)
            };
            return Partial("AddBrand", model);
        }
        public async Task<IActionResult> OnPostAddBrand(AddBrandCategoryViewModel model)
        {
            if (model.CategoryId < 1)
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));


            if (model.Brands.Count < 1)
                return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage));

            var _Category = await _categoryService.GetCategoryWithItsBrands(model.CategoryId);
            if (_Category is null)
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

            _Category.CategoryBrands.Clear();
            model.Brands = model.Brands.Distinct().ToList();
            var brandIdes = await _brandService.GetBrandIdsByList(model.Brands);
            brandIdes.ForEach(bid=>_Category.CategoryBrands.Add(new CategoryBrand()
            {
                BrandId = bid,
                
            }));

            await _unitOfWork.SaveChangesAsync();
            return Json(new JsonResultOperation(true, "برند های مورد نظر با موفقیت به دسته بندی مذکور اضافه گردید"));
        }

        public async Task<IActionResult> OnGetAutocompleteSearch(string term)
        {
            return Json(await _brandService.AutocompleteSearch(term));
        }



        public async Task<IActionResult> OnPostRestore(long elementId)
        {
            var _category = await _categoryService.FindByIdAsync(elementId);
            if (_category is null)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }
            await _categoryService.Restore(_category);
            await _unitOfWork.SaveChangesAsync();
            return Json(new JsonResultOperation(true, "دسته بندی مورد نظر با موفقیت بازگردانی شد"));
        }

        public async Task<IActionResult> OnPostDeletePicture(long elementId)
        {
            var _category = await _categoryService.FindByIdAsync(elementId);
            if (_category is null)
            {
                return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));
            }

            var fileName = _category.Picture;
            _category.Picture = null;
            await _unitOfWork.SaveChangesAsync();
            _uploadFileService.DeleteFile(fileName, "images", "Categories");
            return Json(new JsonResultOperation(true, "تصویر دسته بندی مورد نظر با موفقیت حذف شد"));
        }




        public async Task<IActionResult> OnPostCheckForTitle(string title)
        {
            return Json(!await _categoryService.IsExistsBy(nameof(Entities.Category.Title), title));
        }

        public async Task<IActionResult> OnPostCheckForSlug(string slug)
        {
            return Json(!await _categoryService.IsExistsBy(nameof(Entities.Category.Slug), slug));
        }

        public async Task<IActionResult> OnPostCheckForTitleOnEdit(string title, long id)
        {
            return Json(!await _categoryService.IsExistsBy(nameof(Entities.Category.Title), title, id));
        }

        public async Task<IActionResult> OnPostCheckForSlugOnEdit(string slug, long id)
        {
            return Json(!await _categoryService.IsExistsBy(nameof(Entities.Category.Slug), slug, id));
        }
    }
}
