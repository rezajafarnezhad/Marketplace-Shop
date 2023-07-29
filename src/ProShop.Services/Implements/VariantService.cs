using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.Common.Helpers;
using ProShop.DataLayer.Context;
using ProShop.DataLayer.Migrations;
using ProShop.Entities;
using ProShop.ViewModels.Veriants;

namespace ProShop.Services.Implements;

public class VariantService : GenericService<Variant>, IVariantService
{

    private readonly DbSet<Variant> _variants;
    private readonly DbSet<Product> _product;
    private readonly IMapper _mapper;
    public VariantService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _variants = uow.Set<Entities.Variant>();
        _product = uow.Set<Entities.Product>();
        _mapper = mapper;
    }


    public async Task<ShowVeriantsViewModel> GetVariants(ShowVeriantsViewModel model)
    {
        var Variants = _variants.AsNoTracking().AsQueryable();


        #region Search

        Variants = Variants.CreateSearchExpressions(model.SearchVeriants, callDeletedStatusExpression: false);

        #endregion

        #region Sorting
        Variants = Variants.CreateOrderByExpression(model.SearchVeriants.Sorting.ToString(), model.SearchVeriants.SortingOrder.ToString());

        #endregion

        var paginationResult = await GenericPagination(Variants, model.Pagination);

        return new ShowVeriantsViewModel()
        {
            Veriants = await _mapper.ProjectTo<ShowVeriantViewModel>(paginationResult.Query).ToListAsync(),
            Pagination = paginationResult.Pagination

        };
    }

    public async Task<(bool result,bool isVariantNull)> checkProductAndVariantTypeForAddVariant(long productId , long variantId)
    {
        var product = await _product.Select(c => new { c.Id, c.Category.IsVariantColor }).SingleOrDefaultAsync(c => c.Id == productId);
        
        if (product is null)
            return (false,default);

        if (product.IsVariantColor is null)
            return (true, true);

        var variant = await _variants.Where(c => c.IsConfirmed)
            .Select(c => new { c.Id, c.IsColor }).SingleOrDefaultAsync(c => c.Id == variantId);
        
        if (variant is null)
            return (false,false);

        return (product.IsVariantColor == variant.IsColor,false);

    }

    public async  Task<List<ShowVariantInEditCategoryVariantViewModel>> GetVariantsForEditCategoryVariants(bool isColor)
    {
        return await _mapper.ProjectTo<ShowVariantInEditCategoryVariantViewModel>
            (_variants.Where(c=>c.IsConfirmed).Where(c=>c.IsColor == isColor)).ToListAsync();
    }

    public async Task<bool> CheckVariantsCountAddConfirmStatusForEditCategoryVariants(List<long> variantsIds , bool isColor)
    {
        var result = await _variants
            .Where(c => variantsIds.Contains(c.Id))
            .Where(c=>c.IsColor == isColor)
            .CountAsync(c => c.IsConfirmed);

        return variantsIds.Count == result;
    }
}
