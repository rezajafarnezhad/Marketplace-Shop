using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Utilities.Zlib;

namespace ProShop.Services.Contracts.Identity
{
    public interface ISmsSender
    {

        Task<bool> SendSmsAsync(string number, string message);

    }
}
