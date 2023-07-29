using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using ProShop.Common.Helpers;
using ProShop.Common.IdentityToolkit;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels;
using ProShop.ViewModels.Categories;

namespace ProShop.Services.Implements;

public class CategoryService : GenericService<Category>, ICategoryService
{

    private readonly DbSet<Category> _categories;
    private readonly DbSet<Product> _products;
    private readonly IHttpContextAccessor _httpContext;
    private readonly ISellerService _sellerService;


    public CategoryService(IUnitOfWork uow, IHttpContextAccessor httpContext, ISellerService sellerService) : base(uow)
    {
        _categories = uow.Set<Category>();
        _products = uow.Set<Product>();
        _httpContext = httpContext;
        _sellerService = sellerService;
    }


    public async Task<ShowCategoriesViewModel> GetCategories(ShowCategoriesViewModel model)
    {
        var categories = _categories.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(model.SearchCategories.Title))
            categories = categories.Where(c => c.Title.Contains(model.SearchCategories.Title.Trim()));

        if (!string.IsNullOrWhiteSpace(model.SearchCategories.Slug))
            categories = categories.Where(c => c.Slug.Contains(model.SearchCategories.Slug.Trim()));

        switch (model.SearchCategories.DeletedStatus)
        {
            case DeletedStatus.True:
                break;

            case DeletedStatus.OnlyDeleted:
                categories = categories.Where(c => c.IsDeleted);
                break;

            default:
                categories = categories.Where(c => !c.IsDeleted);
                break;

        }

        switch (model.SearchCategories.ShowInMenusStatus)
        {
            case ShowInMenusStatus.False:
                categories = categories.Where(c => !c.IsShowInMenus);
                break;

            case ShowInMenusStatus.True:
                categories = categories.Where(c => c.IsShowInMenus);
                break;

            default:
                break;
        }

        categories = categories.CreateOrderByExpression(model.SearchCategories.Sorting.ToString(),
            model.SearchCategories.SortingOrder.ToString());

        var paginationResult = await GenericPagination(categories, model.Pagination);

        return new()
        {
            Categories = await paginationResult.Query
                .Select(c => new ShowCategoryViewModel()
                {
                    Id = c.Id,
                    Parent = c.ParentId != null ? c.ParentCategory.Title : "دسته اصلی",
                    ShowInMenus = c.IsShowInMenus,
                    Title = c.Title,
                    slug = c.Slug,
                    Picture = c.Picture ?? "بدون عکس",
                    IsDeleted = c.IsDeleted,
                    ShowEditVariantButton=c.IsVariantColor==null?false:true,
                }).ToListAsync(),
            Pagination = paginationResult.Pagination
        };
    }

    public Dictionary<long, string> GetCategoriesToShowInSelectBox(long? id = null)
    {
        return _categories

            .Where(c => id == null || c.Id != id)
            .ToDictionary(c => c.Id, c => c.Title);
    }

    public override async Task<DuplicateColumns> AddAsync(Category entity)
    {
        var result = new List<string>();

        if (await _categories.AnyAsync(c => c.Title == entity.Title))
            result.Add(nameof(Category.Title));

        if (await _categories.AnyAsync(c => c.Slug == entity.Slug))
            result.Add(nameof(Category.Slug));

        if (!result.Any())
            await base.AddAsync(entity);

        return new DuplicateColumns(!result.Any())
        {
            Columns = result
        };

    }


    public async Task<EditCategoryViewModel> GetForEdit(long Id)
    {
        return await _categories.Select(c => new EditCategoryViewModel()
        {
            Id = c.Id,
            Title = c.Title,
            Slug = c.Slug,
            ParentId = c.ParentId,
            Description = c.Description,
            IsShowInMenus = c.IsShowInMenus,
            CanAddFakeProduct = c.CanAddFakeProduct,
            SelectedPicture = c.Picture,
            IsVariantColor=c.IsVariantColor,
            CanVariantTypeChange = c.categoryVarieants.Any()?false:(!c.HasVariant),

        }).SingleOrDefaultAsync(c => c.Id == Id);
    }


    public override async Task<DuplicateColumns> Update(Category entity)
    {
        var query = _categories.Where(c => c.Id != entity.Id);
        var result = new List<string>();

        if (await query.AnyAsync(c => c.Title == entity.Title))
            result.Add(nameof(Category.Title));

        if (await query.AnyAsync(c => c.Slug == entity.Slug))
            result.Add(nameof(Category.Slug));

        if (!result.Any())
            await base.Update(entity);

        return new DuplicateColumns(!result.Any())
        {
            Columns = result
        };

    }


    public async Task<List<List<ShowCategoryForCreateProductViewModel>>> GetCategoriesForCreateProduct(long[] selectedCategoriesIds)
    {
        var result = new List<List<ShowCategoryForCreateProductViewModel>>();

        result.Add(await _categories.Where(c => c.ParentId == null)
            .Select(c => new ShowCategoryForCreateProductViewModel()
            {
                Id = c.Id,
                Title = c.Title,
                HasChild = c.ChildCategory.Any()
            }).ToListAsync());

        for (var counter = 0; counter < selectedCategoriesIds.Length; counter++)
        {
            var selectedCategoryId = selectedCategoriesIds[counter];
            result.Add(await _categories.Where(c => c.ParentId == selectedCategoryId)
                .Select(c => new ShowCategoryForCreateProductViewModel()
                {
                    Id = c.Id,
                    Title = c.Title,
                    HasChild = c.ChildCategory.Any()
                }).ToListAsync());
        }
        return result;
    }

    public async Task<List<string>> GetCategoryBrands(long categoryId)
    {
        return await _categories
            .SelectMany(c => c.CategoryBrands)
            .Where(c => c.CategoryId == categoryId)
            .Select(c => c.Brand.TitleFa + " " + c.Brand.TitleEn + "|||" + c.CommissionPercentage).ToListAsync();
    }

    public async Task<Dictionary<long, string>> GetCategoriesWithNoChild()
    {
        return await _categories.Where(c => !c.ChildCategory.Any())
            .ToDictionaryAsync(c => c.Id, c => c.Title);
    }


    public async Task<Category> GetCategoryWithItsBrands(long categotyId)
    {
        return await _categories.Include(c => c.CategoryBrands)
            .SingleOrDefaultAsync(c => c.Id == categotyId);
    }

    public async Task<bool> CanAddFakeProduct(long categoryId)
    {
        return await _categories.Where(c => c.Id == categoryId).AnyAsync(c => c.CanAddFakeProduct);
    }


    public async Task<(bool issuccessful, List<long> categoryIds)> GetCategoryParentIds(long categoryId)
    {

        if (!await IsExistsBy(nameof(Entities.Category.Id), categoryId))
            return (false, new List<long>());

        if (await _categories.AnyAsync(c => c.ParentId == categoryId))
            return (false, new List<long>());


        var result = new List<long>() { categoryId };
        var currentCategoryId = categoryId;
        while (true)
        {
            var categoryToAdd = await _categories.Select(c => new { c.Id, c.ParentId }).SingleOrDefaultAsync(c => c.Id == currentCategoryId);
            if (categoryToAdd.ParentId is null)
            {
                break;
            }

            currentCategoryId = categoryToAdd.ParentId.Value;
            result.Add(categoryToAdd.ParentId.Value);
        }

        return (true, result);
    }

    public async Task<Dictionary<long, string>> GetSellerCategories()
    {
        var userId = _httpContext.HttpContext.User.Identity.GetLoggedUserId();
        var SellerId = await _sellerService.GetSellerId(userId);

        return await _products.Where(c => c.SelerId == SellerId)
            .GroupBy(c => c.MainCategoryId)
            .Select(c => new
            {
                c.Key,
                c.First().Category.Title

            }).ToDictionaryAsync(c => c.Key,c=>c.Title);
    }

    public async Task<bool?> IsVariantTypeColor(long categoryId)
    {
        return await _categories.Where(c => c.Id == categoryId).Select(c=>c.IsVariantColor).SingleOrDefaultAsync();

    }
 
    public async Task<Entities.Category> GetCategoryForEditVariant(long catagoryId)
    {
        return await _categories.Include(c=>c.categoryVarieants).SingleOrDefaultAsync(c => c.Id == catagoryId);
    }

}

