using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ProShop.Services.Contracts
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> SendAsync(string url, HttpMethod method,
            Dictionary<string, string> headers = null,
            string content = "", string mediaType = MediaTypeNames.Application.Json);
    }
}