using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.ViewModels.Garantee;

namespace ProShop.Services.Implements;

public class GaranteeService : GenericService<Entities.Garantee>, IGaranteeService
{

    private readonly DbSet<Entities.Garantee> _garanties;
    private readonly IMapper _mapper;

    public GaranteeService(IMapper mapper,IUnitOfWork uow):base(uow)
    {
        _mapper = mapper;
        _garanties = uow.Set<Entities.Garantee>();
    }

    public async Task<ShowGarantiesViewModel> GetGaranties(ShowGarantiesViewModel model)
    {

        var Garanties = _garanties.AsNoTracking().AsQueryable();

        #region Search
        Garanties = Garanties.CreateSearchExpressions(model.SearchGarantiee, callDeletedStatusExpression: false);
        #endregion

        #region Sorting
        Garanties = Garanties.CreateOrderByExpression(model.SearchGarantiee.Sorting.ToString(), model.SearchGarantiee.SortingOrder.ToString());
        #endregion

        var paginationResult = await GenericPagination(Garanties, model.Pagination);


        return new ShowGarantiesViewModel()
        {
            Garanties = await _mapper.ProjectTo<ShowGarantieeViewModel>(paginationResult.Query).ToListAsync(),
            Pagination = paginationResult.Pagination,
        };
    }
}