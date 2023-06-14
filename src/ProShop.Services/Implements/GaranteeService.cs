using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.ViewModels;
using ProShop.ViewModels.Garantee;

namespace ProShop.Services.Implements;

public class GaranteeService : GenericService<Entities.Garantee>, IGaranteeService
{

    private readonly DbSet<Entities.Garantee> _garanties;
    private readonly IMapper _mapper;

    public GaranteeService(IMapper mapper, IUnitOfWork uow) : base(uow)
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

    public async Task<Dictionary<long, string>> GetGaranteesForAddProductVaraint()
    {
        return await _garanties.ToDictionaryAsync(c => c.Id, c => c.FullTitle);
    }

    public List<ShowSelect2DataByAjaxViewModel> SearchOnGatanteesForSelect2Ajax(string input)
    {

        var data = _garanties.Where(c => c.Title.Contains(input)).Where(c=>c.IsConfirmed).Select(c => new ShowSelect2DataByAjaxViewModel
        {
            Id = c.Id,
            Text = c.FullTitle

        })
            .OrderBy(c => c.Id)
            .Take(15)
            .ToList();

        return data;
    }
}