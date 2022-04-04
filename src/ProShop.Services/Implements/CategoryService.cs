using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels;
using ProShop.ViewModels.Categories;

namespace ProShop.Services.Implements;

public class CategoryService : GenericService<Category>, ICategoryService
{

    private readonly DbSet<Category> _categories;
    public CategoryService(IUnitOfWork uow)
        : base(uow)
    {
        _categories = uow.Set<Category>();
    }


    public async Task<ShowCategoriesViewModel> GetCategories(SearchCategoriesViewModel search)
    {
        var categories = _categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.Title))
            categories = categories.Where(c => c.Title.Contains(search.Title.Trim()));

        if (!string.IsNullOrWhiteSpace(search.Slug))
            categories = categories.Where(c => c.Slug.Contains(search.Slug.Trim()));

        switch (search.DeletedStatus)
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

        switch (search.ShowInMenusStatus)
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


        return new()
        {
            Categories = await categories
                .Select(c => new ShowCategoryViewModel()
                {
                    Parent = c.ParentId != null ? c.ParentCategory.Title : "دسته اصلی",
                    ShowInMenus = c.IsShowInMenus,
                    Title = c.Title,
                    slug = c.Slug,

                }).ToListAsync()

        };
    }
}