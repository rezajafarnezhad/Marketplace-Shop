using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProShop.web.Pages.Product
{
    public class IndexModel : PageModel
    {
        public void OnGet(int productCode , string slug)
        {
            ViewData["Slug"]= slug;
            ViewData["ProductCode"]= productCode;
        }
    }
}
