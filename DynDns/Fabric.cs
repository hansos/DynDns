using DynDns.Ip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynDns
{
    internal class Fabric
    {



        internal Fabric()
        {
            try
            {
                Log.CreateDirectoryIfNeeded();
                Log.WriteLine(Log.TraceLevel.Success, "Fabric.Fabric", "Program started.");
            }
            catch (Exception ex)
            {
                Log.WriteLine(Log.TraceLevel.Error, "Fabric.Fabric", "Unspecified error", ex);
            }

        }

        /// <summary>
        /// Compare detected public IP address with stored address.
        /// If changed, update DNS server and stored addresses.
        /// </summary>
        /// <returns></returns>
        internal bool Run()
        {
            try
            {

                // Check if We can get our IP from a DynDns.org or other provider.
                // If a valid IP is not received, return false.
                string actualIp = DnsFinder.WhatismyipaddressCom;
                if (DnsFinder.IsValidIpv4(actualIp)) 
                {
                    Log.WriteLine(Log.TraceLevel.Success, "Fabric.Run", $"Actual IP '{actualIp}' is a valid IP.");
                }
                else
                {
                    Log.WriteLine(Log.TraceLevel.Success, "Fabric.Run", $"Did not find a valid IP for this computer.");
                    return false;
                }


                // Check for a valid dyndns.dat locally. If not found, create an empty file.
                // Load the stored IP from the data file.
                Data.DataEngine dataEngine = new();
                Data.DynDnsData dynDnsData = dataEngine.ReadDataFile(ExecDirectory);


                // Compare local IP with actual IP.
                // If equal, we don't need to do anything. return true.
                if (actualIp.Equals(dynDnsData.CurrentIp))
                {
                    Log.WriteLine(Log.TraceLevel.Success, "Fabric.Run", $"Actual IP '{actualIp}' is equal to stored IP '{dynDnsData.CurrentIp}'. No need for updates.");
                    return true;
                }
                else
                {
                    Log.WriteLine(Log.TraceLevel.Success, "Fabric.Run", $"Actual IP '{actualIp}' differs from stored IP '{dynDnsData.CurrentIp}'. Need to update DNS servers and local data file.");
                }


                // Update the AWS DNS server according with zone records read from zones.dat.
                Zone.ZoneEngine zoneEngine = new();
                List<Zone.ZoneRecord> zoneRecords = zoneEngine.LoadZones(ExecDirectory);
                foreach(var zoneRecord in zoneRecords)
                {
                    Log.WriteLine(Log.TraceLevel.Success, "Fabric.Run", $"Updating zone '{zoneRecord.ZoneId}', record '{zoneRecord.RecordName.Trim()}' with IP '{actualIp}'.");
                    IpEngine ipEngine = new();
                    ipEngine.ChangeResourceRecordSet(zoneRecord.ZoneId, zoneRecord.RecordName, actualIp);
                }


                //Update the local data file with actual IP .
                dynDnsData.CurrentIp = actualIp;
                dataEngine.WriteDataFile(ExecDirectory, dynDnsData);


                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(Log.TraceLevel.Error, "Fabric.Run", $"Error running Engine: ",ex);
                throw;
            }
        }



        private string ExecDirectory
        {
            get
            {
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return "/opt/dyndns";
                }
                else
                {
                    return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                }
            }
        }

    }
}
