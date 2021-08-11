using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace MyIp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MyIpController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return GetUserIPAddress();
        }


        private string GetUserIPAddress()
        {

            string result;
            if (Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedIps))
            {
                result = forwardedIps.First();
            }
            else
            {
                result = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return result;
        }

    }
}
