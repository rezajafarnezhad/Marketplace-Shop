using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.ViewModels;
using ProShop.ViewModels.Brands;

namespace ProShop.Services.Implements;

public class BrandService : GenericService<Brand>, IBrandService
{

    private readonly DbSet<Brand> _brands;
    private readonly IMapper _mapper;
    public BrandService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
        _brands = uow.Set<Brand>();
    }


    public async Task<ShowBrandsViewModel> GetBrands(ShowBrandsViewModel model)
    {
        var brands = _brands.AsNoTracking().AsQueryable();

        #region Search

        if (!string.IsNullOrWhiteSpace(model.SearchBrands.TitleFa))
            brands = _brands.Where(c => c.TitleFa.Contains(model.SearchBrands.TitleFa.Trim()));
        
        if (!string.IsNullOrWhiteSpace(model.SearchBrands.TitleEn))
            brands = _brands.Where(c => c.TitleEn.Contains(model.SearchBrands.TitleEn.Trim())); 
        
        if (!string.IsNullOrWhiteSpace(model.SearchBrands.BrandLinkEn))
            brands = _brands.Where(c => c.BrandLinkEn.Contains(model.SearchBrands.BrandLinkEn.Trim()));


        if (model.SearchBrands.IsConfirmed != null)
            brands = brands.Where(c => c.IsConfirmed == model.SearchBrands.IsConfirmed.Value);

        switch (model.SearchBrands.DeletedStatus)
        {

            case DeletedStatus.True:
                break;

            case DeletedStatus.OnlyDeleted:
                brands = brands.Where(c => c.IsDeleted);
                break;

            default:
                brands = brands.Where(c => !c.IsDeleted);
                break;
        }
       

        if (model.SearchBrands.IsIranianBrand != null)
            brands = brands.Where(c => c.IsIranianBrand == model.SearchBrands.IsIranianBrand.Value);
        

        #region Sorting

        brands = brands.CreateOrderByExpression(model.SearchBrands.Sorting.ToString(),
            model.SearchBrands.SortingOrder.ToString());

        #endregion


        var paginationResult = await GenericPagination(brands, model.Pagination);

        #endregion


        return new ShowBrandsViewModel()
        {
            Brands = await _mapper.ProjectTo<ShowBrandViewModel>(paginationResult.Query).ToListAsync(),
            Pagination = paginationResult.Pagination
        };
    }

    public override async Task<DuplicateColumns> AddAsync(Brand entity)
    {
        var result = new List<string>();

        if (await _brands.AnyAsync(c => c.TitleFa == entity.TitleFa))
            result.Add(nameof(Brand.TitleFa)); 
        
        if (await _brands.AnyAsync(c => c.TitleEn == entity.TitleEn))
            result.Add(nameof(Brand.TitleEn));

        if (!result.Any())
            await base.AddAsync(entity);

        return new DuplicateColumns(!result.Any())
        {
            Columns = result
        };
    }

    public async Task<EditBrandViewModel> GetForEdit(long Id)
    {
        return await _mapper.ProjectTo<EditBrandViewModel>(_brands)
            .SingleOrDefaultAsync(c => c.Id == Id);
    }

    public override async Task<DuplicateColumns> Update(Brand entity)
    {
        var query = _brands.Where(c => c.Id != entity.Id);
        var result = new List<string>();

        if (await query.AnyAsync(c => c.TitleFa == entity.TitleFa))
            result.Add(nameof(Brand.TitleFa));

        if (await query.AnyAsync(c => c.TitleEn == entity.TitleEn))
            result.Add(nameof(Brand.TitleEn));

        if (!result.Any())
            await base.Update(entity);

        return new DuplicateColumns(!result.Any())
        {
            Columns = result
        };

    }

    public async Task<List<string>> AutocompleteSearch(string term)
    {
        return await _brands.Where(c => c.TitleFa.Contains(term) || c.TitleEn.Contains(term))
            .Take(20)
            .Select(c => c.TitleFa + " " + c.TitleEn).ToListAsync();
    }

    public async Task<List<long>> GetBrandIdsByList(List<string> brandsTile)
    {

        return await _brands.Where(c => brandsTile.Contains(c.TitleFa + " " + c.TitleEn))
            .Select(c => c.Id)
            .ToListAsync();

    }
    public Task<Dictionary<long, string>> GetBrandsByCategoryId(long categoryId)
    {
        return _brands.SelectMany(c => c.CategoryBrands)
            .Where(c => c.CategoryId == categoryId)
            .Include(c=>c.Brand)
            .ToDictionaryAsync(c => c.BrandId, c => c.Brand.TitleFa + " " + c.Brand.TitleEn);
    }

    public async Task<BrandDetailsViewModel> GetBrandDetails(long brandId)
    {
        return await _mapper.ProjectTo<BrandDetailsViewModel>(_brands)
            .SingleOrDefaultAsync(c => c.Id == brandId);
    }

    public async Task<Brand> GetInActiveBrand(long brandId)
    {
        return await _brands.Where(c => c.IsConfirmed == false)
            .SingleOrDefaultAsync(c => c.Id == brandId);
    }
}