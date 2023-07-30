using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.Common.Helpers;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
using ProShop.ViewModels.Brands;
using ProShop.ViewModels.Orders;
using System.Data;

namespace ProShop.Services.Implements;

public class OrderService : GenericService<Entities.Order>, IOrderService
{
    private readonly DbSet<Entities.Order> _orders;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
        _orders = uow.Set<Entities.Order>();
    }

    public async Task<Order> FindByOrderNumberAndIncludeParcelPosts(long orderNumber, long userId)
    {
        return await _orders
            .Include(c => c.ParcalPosts)
            .Where(c => c.OrderNumber == orderNumber)
            .SingleOrDefaultAsync(c => c.UserId == userId);
    }


    public async Task<ShowOrdersViewModel> GetOrders(ShowOrdersViewModel model)
    {
        var orders = _orders.AsNoTracking().AsQueryable();

        #region Search

        if(model.SearchOrders.OnlyPayedOrders)
            orders = orders.Where(c => c.IsPay);


        var searchFullName = model.SearchOrders.FullName;
        if (!string.IsNullOrWhiteSpace(searchFullName))
            orders = orders.Where(c => (c.Address.FirstName+" "+c.Address.LastName).Contains(searchFullName));


        var searchProvinceId = model.SearchOrders.ProvinceId;
        if(searchProvinceId is > 0 )
            orders = orders.Where(c => c.Address.ProvinceId == searchProvinceId);

        var searchCityId = model.SearchOrders.CityId;
        if (searchCityId is > 0)
            orders = orders.Where(c => c.Address.CityId == searchCityId);

        orders = orders.CreateSearchExpressions(model.SearchOrders, false);

        #region Sorting

        orders = orders.CreateOrderByExpression(model.SearchOrders.Sorting.ToString(),
            model.SearchOrders.SortingOrder.ToString());

        #endregion


        var paginationResult = await GenericPagination(orders, model.Pagination);

        #endregion


        return new ShowOrdersViewModel()
        {
            Orders = await _mapper.ProjectTo<ShowOrder>(paginationResult.Query).ToListAsync(),
            Pagination = paginationResult.Pagination
        };
    }

    public  Task<OrderDetailsViewModel> GetOrderDetails(long orderId)
    {
        return _mapper.ProjectTo<OrderDetailsViewModel>(_orders.AsSplitQuery().AsNoTracking()).SingleOrDefaultAsync(c => c.Id == orderId);
    }


    public async Task<ShowOrdersInDeliveryOrdersViewModel> GetDeliveryOrders(ShowOrdersInDeliveryOrdersViewModel model)
    {
        var orders = _orders.Where(c=>c.IsPay).AsNoTracking().AsQueryable();

        #region Search

        var searchFullName = model.SearchOrders.FullName;
        if (!string.IsNullOrWhiteSpace(searchFullName))
            orders = orders.Where(c => (c.Address.FirstName + " " + c.Address.LastName).Contains(searchFullName));


        var searchProvinceId = model.SearchOrders.ProvinceId;
        if (searchProvinceId is > 0)
            orders = orders.Where(c => c.Address.ProvinceId == searchProvinceId);

        var searchCityId = model.SearchOrders.CityId;
        if (searchCityId is > 0)
            orders = orders.Where(c => c.Address.CityId == searchCityId);

        if(model.SearchOrders.OrderStatus is null)
            orders = orders.Where(c => c.OrderStatus != OrderStatus.WaitingForPaying)
                .Where(c=>c.OrderStatus !=OrderStatus.Processing);


        orders = orders.CreateSearchExpressions(model.SearchOrders, false);

        #region Sorting

        orders = orders.CreateOrderByExpression(model.SearchOrders.Sorting.ToString(),
            model.SearchOrders.SortingOrder.ToString());

        #endregion


        var paginationResult = await GenericPagination(orders, model.Pagination);

        #endregion


        return new ShowOrdersInDeliveryOrdersViewModel()
        {
            Orders = await _mapper.ProjectTo<ShowOrderInDeliveryOrderViewModel>(paginationResult.Query).ToListAsync(),
            Pagination = paginationResult.Pagination
        };
    }
}