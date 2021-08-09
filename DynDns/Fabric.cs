using DynDns.Ip;
using DynDns.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DynDns
{
    internal class Fabric
    {
        private Log.TraceLevel _maxLevel = Log.TraceLevel.Trace;
        private bool _quiet;
        internal Fabric(Log.TraceLevel maxLevel, bool quiet)
        {
            try
            {
                _maxLevel = maxLevel;
                _quiet = quiet;
                Log.CreateDirectoryIfNeeded();
                Log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.Fabric", "Program started.", _quiet);
            }
            catch (Exception ex)
            {
                Log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.Fabric", "Unspecified error", _quiet, ex);
            }

        }

        /// <summary>
        /// Compare detected public IP address with stored address.
        /// If changed, update DNS server and stored addresses.
        /// </summary>
        /// <returns></returns>
        internal bool Run(bool sharp, string dataFile, string tracePath)
        {
            try
            {

                // Check if We can get our IP from a DynDns.org or other provider.
                // If a valid IP is not received, return false.
                string actualIp = DnsFinder.WhatismyipaddressCom(_maxLevel, _quiet);
                if (DnsFinder.IsValidIpv4(actualIp)) 
                {
                    string msg = $"Actual IP '{actualIp}' is a valid IP.";
                    Log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.Run", msg, _quiet);
                }
                else
                {
                    Log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.Run", $"Did not find a valid IP for this computer.", _quiet);
                    return false;
                }


                // Check for a valid dyndns.dat locally. If not found, create an empty file.
                // Load the stored IP from the data file.
                Data.DataEngine dataEngine = new(_maxLevel, _quiet);
                Data.DynDnsData dynDnsData = dataEngine.ReadDataFile(ExecDirectory);


                // Compare local IP with actual IP.
                // If equal, we don't need to do anything. return true.
                if (actualIp.Equals(dynDnsData.CurrentIp))
                {
                    string msg = $"Actual IP '{actualIp}' is equal to stored IP '{dynDnsData.CurrentIp}'. No need for updates.";
                    Log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.Run", msg, _quiet);
                    return true;
                }
                else
                {
                    string msg = $"Actual IP '{actualIp}' differs from stored IP '{dynDnsData.CurrentIp}'. Need to update DNS servers and local data file.";
                    Log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.Run", msg, _quiet);
                }


                // Update the AWS DNS server according with zone records read from zones.dat.
                Zone.ZoneEngine zoneEngine = new(_maxLevel, _quiet);
                List<Zone.ZoneRecord> zoneRecords = zoneEngine.LoadZones(ExecDirectory);
                foreach(var zoneRecord in zoneRecords)
                {
                    string msg = $"Updating zone '{zoneRecord.ZoneId}', record '{zoneRecord.RecordName.Trim()}' with IP '{actualIp}'.";
                    Log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.Run", msg, _quiet);
                    IpEngine ipEngine = new(_maxLevel, _quiet);
                    ipEngine.ChangeResourceRecordSet(zoneRecord.ZoneId, zoneRecord.RecordName, actualIp, dynDnsData.CurrentIp, sharp);
                }


                //Update the local data file with actual IP .
                dynDnsData.CurrentIp = actualIp;
                dataEngine.WriteDataFile(ExecDirectory, dynDnsData);


                return true;
            }
            catch (Exception ex)
            {
                Log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.Run", $"Error running Engine: ", _quiet, ex);
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
