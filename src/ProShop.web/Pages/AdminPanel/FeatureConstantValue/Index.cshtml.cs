using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;
using ProShop.ViewModels.FeatureConstantValue;

namespace ProShop.web.Pages.AdminPanel.FeatureConstantValue;

public class IndexModel : PageBase
{
    private readonly IFeatureConstantValuesService _featureConstantValuesService;
    private readonly ICategoryService _categoryService;
    private readonly ICategoryFeatureService _categoryFeatureService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public IndexModel(IFeatureConstantValuesService featureConstantValuesService, ICategoryService categoryService, ICategoryFeatureService categoryFeatureService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _featureConstantValuesService = featureConstantValuesService;
        _categoryService = categoryService;
        _categoryFeatureService = categoryFeatureService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    [BindProperty(SupportsGet = true)]
    public ShowFeatureConstantValuesViewModel ShowConstantValues { get; set; } = new();
    public void OnGet()
    {
        var Categories = _categoryService.GetCategoriesToShowInSelectBox();
        ShowConstantValues.SearchFeatureConstant.Categories = Categories.CreateSelectListItem();
    }

    public async Task<IActionResult> OnGetGetDataTableAsync()
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(String.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }
        return Partial("_List", await _featureConstantValuesService.GetFeatureConstants(ShowConstantValues));
    }

    public async Task<IActionResult> OnGetGetCategoryFeatures(long categotyId)
    {
        if (categotyId < 1)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));


        return Json(new JsonResultOperation(true, "")
        {
            Data = await _categoryFeatureService.GetCategoryFeatureBy(categotyId)
        });
    }

    public async Task<IActionResult> OnPostDelete(long Id)
    {
        var data = await _featureConstantValuesService.FindByIdAsync(Id);
        if (data is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        _featureConstantValuesService.Remove(data);
        await _unitOfWork.SaveChangesAsync();
        return Json(new JsonResultOperation(true, "مقدار ثابت ویژگی با موفقیت حذف شد"));
    }

    public IActionResult OnGetAdd()
    {
        var model = new AddFeatureConstantValue()
        {
            Categories = _categoryService.GetCategoriesToShowInSelectBox().CreateSelectListItem(),
        };

        return Partial("Add", model);
    }

    public async Task<IActionResult> OnPostAdd(AddFeatureConstantValue model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(String.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        if (!await _categoryFeatureService.CheckCategoryFeature(model.CategoryId, model.FeatureId))
            return Json(new JsonResultOperation(false));


        var ConstantValue = _mapper.Map<Entities.FeatureConstantValue>(model);

        await _featureConstantValuesService.AddAsync(ConstantValue);
        await _unitOfWork.SaveChangesAsync();

        return Json(new JsonResultOperation(true, "مقدار ثابت ویژگی با موفقیت اضافه شد"));
    }

    public async Task<IActionResult> OnGetEdit(long Id)
    {
        var constantValue = await _featureConstantValuesService.FindByIdAsync(Id);

        if (constantValue is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        var Categories = _categoryService.GetCategoriesToShowInSelectBox();
        var features = await _categoryFeatureService.GetCategoryFeatureBy(constantValue.CategoryId);
        var model = _mapper.Map<EditFeatureConstantValue>(constantValue);

        model.Categories = Categories.CreateSelectListItem();
        model.Features = features.CreateSelectListItem();

        return Partial("Edit",model);
    }


    public async Task<IActionResult> OnPostEdit(EditFeatureConstantValue model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(String.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        var _FeatureConstantValue = await _featureConstantValuesService.FindByIdAsync(model.Id);
        if (_FeatureConstantValue is null)
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundErrorMessage));

        _FeatureConstantValue = _mapper.Map(model, _FeatureConstantValue);
        _featureConstantValuesService.Update(_FeatureConstantValue);
        await _unitOfWork.SaveChangesAsync();

        return Json(new JsonResultOperation(true, "مقدار ثابت ویژگی با موفقیت ویرایش شد"));
    }
}