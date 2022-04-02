using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProShop.web.Pages;

public class PageBase : PageModel
{
    public JsonResult Json(object input)
    {
        return new(input);
    }
}

