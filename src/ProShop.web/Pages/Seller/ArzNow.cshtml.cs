using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ProShop.web.Pages.Seller;

public class ArzNowModel : PageModel
{
    public etelasat Etelasat { get; set; } = new etelasat();
    public IActionResult OnGet()
    {

        var UrlArzApi = "http://api.codebazan.ir/arz/?type=arz";
        var UrlProxyApi = "http://api.codebazan.ir/mtproto/json/";

        using (var client = new HttpClient())
        {
            var ser = client.GetStringAsync(UrlArzApi).Result;
            Etelasat.Arz = JsonConvert.DeserializeObject<Arz>(ser);
        }

        using (var client2 = new HttpClient())
        {
            var ser2 = client2.GetStringAsync(UrlProxyApi).Result;
            Etelasat.Prooxy = JsonConvert.DeserializeObject<Proxy>(ser2);
        }


        return Page();

    }
}


public class etelasat
{
    public Arz Arz { get; set; }
    public Proxy Prooxy { get; set; }
}


public class ProxyResult
{
    public string server { get; set; }
    public string port { get; set; }
    public string secret { get; set; }
}

public class Proxy
{
    public bool Ok { get; set; }
    public int tedad { get; set; }
    public List<ProxyResult> Result { get; set; } = new();
}



public class Arz
{
    public bool Ok { get; set; }
    public List<ArzResult> Result { get; set; } = new List<ArzResult>();
}
public class ArzResult
{
    public string name { get; set; }
    public string price { get; set; }
    public string change { get; set; }
    public int percent { get; set; }
    public string low { get; set; }
    public string High { get; set; }
    public string update { get; set; }
}
