using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Services.Contracts;

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

    public async Task<int> GetOrderNumberForCreateOrderAndPay()
    {
        var LastOrderNumber = await _orders.OrderByDescending(c => c.Id).Select(c=>c.OrderNumber).FirstOrDefaultAsync();
        return LastOrderNumber + 5020;

    }

}