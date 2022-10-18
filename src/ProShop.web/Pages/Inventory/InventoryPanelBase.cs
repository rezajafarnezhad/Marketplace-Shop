using Microsoft.AspNetCore.Authorization;
using ProShop.Services.Implements.Identity;

namespace ProShop.web.Pages.Inventory;

[Authorize(Roles = ConstantRoles.Warehouse)]
public class InventoryPanelBase:PageBase
{
}