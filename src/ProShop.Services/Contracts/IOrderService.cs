using ProShop.Entities;

namespace ProShop.Services.Contracts;

public interface IOrderService : IGenericService<Order>
{
    Task<Order> FindByOrderNumberAndIncludeParcelPosts(long orderNumber , long userId);
}