using DynDns.Logging;
using System;
using System.Net;

namespace DynDns.Ip
{
    internal static class DnsFinder
    {

        internal static string GetHtmlString(string url,  Log.TraceLevel maxLevel, bool quiet, Log log)
        {
            try
            {
                log.WriteTrace(Log.TraceLevel.Trace, maxLevel, "DnsFinder.GetHtmlString", $"Looking for IP at '{url}'.", quiet);
                WebRequest req = WebRequest.Create(url);
                WebResponse resp = req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                string response = sr.ReadToEnd().Trim();
                return response;
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
