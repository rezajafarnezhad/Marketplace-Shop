using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProShop.DataLayer.Context;
using ProShop.Entities;
using ProShop.Services.Contracts;
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
}