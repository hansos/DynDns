using Amazon.Route53;
using Amazon.Route53.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace DynDns
{
    class Program
    {





       // static async Task ExecAsync(string recName)
       // {
       //     try
       //     {
       //         string zoneName = "esogame.net.";
       //         string hostedZoneId = "Z05346433GDXGH3DVNB6C";
       //         //string publicIp = MyPublicIp;

       //         //var result = await AWS.ChangeResourceRecordSet(hostedZoneId, zoneName, recName, publicIp);
       //         string msg = "IP updated successfully on DNS server.";
       //         Console.WriteLine(msg);
       //         Log.WriteLine(msg);
       //     }
       //     catch (Exception ex)
       //     {
       //         Console.WriteLine(ex.Message);
       //         Log.WriteLine(ex.Message);
       //         throw;
       //     }
       //}

        static void Main(string[] args)
        {
            try
            {

                Console.WriteLine("EsoGame Dynamic DNS Updater");
                Console.WriteLine("Copyright (c) 2021 hans.olav@sorteberg.com, all rights reserved.");

                var result = new Fabric().Run();

                //string execPath = Assembly.GetEntryAssembly().Location;
                //Log.WriteLine($"DynDns running in '{execPath}'.");

                //string recName = "www.esogame.net.";
                //string recName = "rygeneminecraftforening.esogame.net.";
                //Log.WriteLine($"Correct IP address for '{recName}' is '{MyPublicIp}'.");

                // _ = ExecAsync(recName);

            }
            catch (Exception ex)
            {
                Log.WriteTrace(Log.TraceLevel.Error, "Fabric.Fabric", $"Program completed with error(s):", ex);
            }


        }
    }
}
