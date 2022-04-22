using Microsoft.AspNetCore.Authorization;
using ProShop.Services.Implements.Identity;

namespace ProShop.web.Pages.SellerPanel;

[Authorize(Roles = ConstantRoles.Seller)]
public class SellerPanelBase:PageBase
{
}