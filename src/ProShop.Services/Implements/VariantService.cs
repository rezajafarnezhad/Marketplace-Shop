using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.DataLayer.Migrations;
using ProShop.Entities;
using ProShop.ViewModels.Veriants;

namespace ProShop.Services.Implements;

public class VariantService : GenericService<Variant>, IVariantService
{

    private readonly DbSet<Variant> _variants;
    private readonly IMapper _mapper;
    public VariantService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _variants = uow.Set<Entities.Variant>();
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
}