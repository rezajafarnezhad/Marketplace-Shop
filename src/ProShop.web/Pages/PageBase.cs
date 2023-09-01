using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProShop.Common.Constants;
using ProShop.Common.Helpers;

namespace ProShop.web.Pages;

public class PageBase : PageModel
{
    public JsonResult Json(object input)
    {
        return new(input);
    }

    public JsonResult RecordNotFound(string message = null)
    {
       return Json(new JsonResultOperation(false,message??PublicConstantStrings.RecordNotFoundErrorMessage));
    }

    public JsonResult jsonBadRequest(string message = null)
    {
        return Json(new JsonResultOperation(false, message ?? "فقط کالاهای که دسته بندی مرتبط دارن"));
    }
}

