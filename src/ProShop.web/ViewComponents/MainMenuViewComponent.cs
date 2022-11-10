using Microsoft.AspNetCore.Mvc;

namespace ProShop.web.ViewComponents;

public class MainMenuViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
