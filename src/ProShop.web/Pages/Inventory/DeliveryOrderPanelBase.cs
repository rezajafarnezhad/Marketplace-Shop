using Microsoft.AspNetCore.Authorization;
using ProShop.Services.Implements.Identity;

namespace ProShop.web.Pages.Inventory;

[Authorize(Roles =ConstantRoles.DeliveryMan)]
public class DeliveryOrderPanelBase : PageBase
{

}