using System;
using System.Net;

namespace DynDns.Ip
{
    internal static class DnsFinder
    {

        internal static string WhatismyipaddressCom
        {
            get
            {
                try
                {
                    string url = "http://bot.whatismyipaddress.com";
                    WebRequest req = WebRequest.Create(url);
                    WebResponse resp = req.GetResponse();
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    string response = sr.ReadToEnd().Trim();
                    return response;
                }
                catch (Exception ex)
                {
                    Log.WriteLine(Log.TraceLevel.Error, "DnsFinder.WhatismyipaddressCom", $"Error finding public IP:", ex);
                    return null;
                }
            }
        }


        internal static string DynDnsOrg
        {
            get
            {
                try
                {
                    string url = "http://checkip.dyndns.org";
                    WebRequest req = WebRequest.Create(url);
                    WebResponse resp = req.GetResponse();
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    string response = sr.ReadToEnd().Trim();
                    string[] a = response.Split(':');
                    string a2 = a[1].Substring(1);
                    string[] a3 = a2.Split('<');
                    Log.WriteLine(Log.TraceLevel.Success, "DnsFinder.MyPublicIp", $"IP retrieved from dyndns.org: '{a3[0]}'");
                    return a3[0];
                }
                catch (Exception ex)
                {
                    Log.WriteLine(Log.TraceLevel.Error, "DnsFinder.DynDnsOrg", $"Error finding public IP:", ex);
                    return null;
                }
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
