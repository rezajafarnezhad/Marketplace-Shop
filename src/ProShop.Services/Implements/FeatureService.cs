using Microsoft.EntityFrameworkCore;
using ProShop.Common.Helpers;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Features;

namespace ProShop.Services.Implements;

public class FeatureService : GenericService<Feature>, IFeatureService
{
    private readonly DbSet<Feature> _features;
    public FeatureService(IUnitOfWork uow)
        : base(uow)
    {
        _features = uow.Set<Feature>();
    }

    public async Task<ShowFeaturesViewModel> GetCategoryFeatures(ShowFeaturesViewModel model)
    {
        var features = _features.AsQueryable();

        features = features.CreateContainsExpressions(model.SearchFeature);

        features = features.SelectMany(x => x.CategoryFeatures)
            .Where(x => x.CategoryId == model.SearchFeature.CategoryId)
            .Select(x => x.Feature);


        features = features.CreateOrderByExpression(model.SearchFeature.Sorting.ToString(),
            model.SearchFeature.SortingOrder.ToString());


        var paginationResult = await GenericPagination(features, model.Pagination);

        return new ShowFeaturesViewModel()
        {
            FeatureViewModels = await paginationResult.Query
                .Select(c => new ShowFeatureViewModel()
                {
                    FeatureId = c.Id,
                    Title = c.Title,
                    CategoryId = model.SearchFeature.CategoryId,


                }).ToListAsync(),
            Pagination = paginationResult.Pagination,

        };
    }

    public async Task<Feature> FindByTitle(string title)
    {
        return await _features.SingleOrDefaultAsync(c => c.Title == title);
    }

    public async Task<List<string>> AutocompleteSearch(string input)
    {
        var data =  await _features.Where(c => c.Title.Contains(input.Trim()))
            .Take(20)
            .Select(c=>c.Title)
            .ToListAsync();
       
        return data;
    }
}