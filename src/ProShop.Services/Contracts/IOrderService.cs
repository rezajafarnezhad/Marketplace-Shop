using ProShop.Entities;

namespace ProShop.Services.Contracts;

public interface IOrderService : IGenericService<Order>
{
    Task<int> GetOrderNumberForCreateOrderAndPay();
}