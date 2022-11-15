using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProShop.Common.Helpers;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Consignments;

namespace ProShop.Services.Implements;

public class ConsignmentService : GenericService<Consignment>, IConsignmentService
{

    private readonly DbSet<Consignment> _consignment;
    private readonly IMapper _mapper;
    public ConsignmentService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _consignment = uow.Set<Entities.Consignment>();
        _mapper = mapper;
    }

    public async Task<ShowConsignmentsViewModel> GetConsignments(ShowConsignmentsViewModel model)
    {
        var Consignment = _consignment.AsNoTracking().AsQueryable();

        #region Search

        var SearchShopName = model.SearchConsignments.ShopName;
        if (!string.IsNullOrWhiteSpace(SearchShopName))
            Consignment = Consignment.Where(c => c.Seller.ShopName.Contains(SearchShopName.Trim()));

        Consignment = Consignment.CreateSearchExpressions(model.SearchConsignments, false);
       
        #endregion

        #region Sorting

        Consignment = Consignment.CreateOrderByExpression(model.SearchConsignments.Sorting.ToString(), model.SearchConsignments.SortingOrder.ToString());

        #endregion

        var paginationResult = await GenericPagination(Consignment, model.Pagination);

        return new ShowConsignmentsViewModel()
        {
            Consignments = await _mapper.ProjectTo<ShowConsignmentViewModel>(paginationResult.Query).ToListAsync(),
            Pagination = paginationResult.Pagination,
        };

    }

    public async Task<Consignment> GetConsignmentForConfirmation(long consignmentId)
    {
        return await _consignment.Where(c => c.ConsignmentStatus == ConsignmentStatus.AwaitingApproval).SingleOrDefaultAsync(c => c.Id == consignmentId);
    }

    public async Task<Consignment> GetConsignmentToReceive(long consignmentId)
    {
        return await _consignment.Where(c => c.ConsignmentStatus == ConsignmentStatus.ConfirmAndAwaitingForConsignment).SingleOrDefaultAsync(c => c.Id == consignmentId);

    }
    public Task<ShowConsignmentDetailsViewModel> GetConsignmentDetails(long consignmentId)
    {
        return _consignment.ProjectTo<ShowConsignmentDetailsViewModel>(
               configuration: _mapper.ConfigurationProvider, parameters: new { consignmentId = consignmentId })
           .SingleOrDefaultAsync(c => c.Id == consignmentId);
    }

    public async Task<bool> IsExistsConsignmetWithReceivedStatus(long consignmentId)
    {
        return await _consignment.Where(c=>c.ConsignmentStatus == ConsignmentStatus.Received).AnyAsync(c => c.Id == consignmentId);
    }

    public async Task<Consignment> GetConsignmentWithReceivedStatus(long consignmentId)
    {
        return await _consignment.Where(c => c.ConsignmentStatus == ConsignmentStatus.Received).SingleOrDefaultAsync(c => c.Id == consignmentId);

    }

    public async Task<bool> CanAddStockForConsignmentItems(long consignmentId)
    {
        return await _consignment.Where(c => c.ConsignmentStatus == ConsignmentStatus.Rejected).AnyAsync(c=>c.Id == consignmentId);
    }
}
