using Microsoft.AspNetCore.Authorization;
using ProShop.Services.Implements.Identity;

namespace ProShop.web.Pages.Inventory;

[Authorize(Roles = $"{ConstantRoles.Warehouse},{ConstantRoles.DeliveryMan}")]
public class InventoryPanelBase:PageBase
{
}