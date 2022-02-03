using DynDns.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DynDns.Ip
{
    internal static class DnsFinder
    {


        internal static async Task<string> GetHtmlString(string url,  Log.TraceLevel maxLevel, bool quiet, Log log)
        {
            try
            {
                HttpClient client = new();
                log.WriteTrace(Log.TraceLevel.Trace, maxLevel, "DnsFinder.GetHtmlString", $"Looking for IP at '{url}'.", quiet);
                var response = client.GetAsync(url);
                response.Result.EnsureSuccessStatusCode();
                string htmlString = await response.Result.Content.ReadAsStringAsync();
                return htmlString;
            }
            catch (Exception ex)
            {
                log.WriteTrace(Log.TraceLevel.Error, maxLevel, "DnsFinder.GetHtmlString", $"Error finding public IP:", quiet, ex);
                return null;
            }
        }

        internal static bool IsValidIpv4(string ip)
        {
            IPAddress address;
            if (IPAddress.TryParse(ip, out address))
            {
                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        return true;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        // V6 is not tested, so we're returning false here for now.
                        return false;
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
