using ProShop.Entities;
using ProShop.ViewModels.Orders;

namespace ProShop.Services.Contracts;

public interface IOrderService : IGenericService<Order>
{
    Task<Order> FindByOrderNumberAndIncludeParcelPosts(long orderNumber , long userId);
    Task<ShowOrdersViewModel> GetOrders(ShowOrdersViewModel model);
    Task<OrderDetailsViewModel> GetOrderDetails(long orderId);
    Task<ShowOrdersInDeliveryOrdersViewModel> GetDeliveryOrders(ShowOrdersInDeliveryOrdersViewModel model);
}