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
        private Log _log;
        internal Fabric(Log.TraceLevel maxLevel, bool quiet, string logFilePath)
        {
            try
            {
                _maxLevel = maxLevel;
                _quiet = quiet;


                _log = new Log(LogFilePath(logFilePath));
                _log.CreateDirectoryIfNeeded();
                _log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.Fabric", "Program started.", _quiet);
            }
            catch (Exception ex)
            {
                _log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.Fabric", "Unspecified error", _quiet, ex);
            }

        }

        /// <summary>
        /// Compare detected public IP address with stored address.
        /// If changed, update DNS server and stored addresses.
        /// </summary>
        /// <returns></returns>
        internal bool Run(bool sharp, string ipBuffer, string zoneFilePath)
        {
            try
            {

                // TODO: tracePath and _log confusing.
                // Check if We can get our IP from a DynDns.org or other provider.
                // If a valid IP is not received, return false.
                string actualIp = DnsFinder.WhatismyipaddressCom(_maxLevel, _quiet, _log);
                if (DnsFinder.IsValidIpv4(actualIp)) 
                {
                    string msg = $"Actual IP '{actualIp}' is a valid IP.";
                    _log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.Run", msg, _quiet);
                }
                else
                {
                    _log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.Run", $"Did not find a valid IP for this computer.", _quiet);
                    return false;
                }


                // Check for a valid dyndns.dat locally. If not found, create an empty file.
                // Load the stored IP from the data file.
                Data.DataEngine dataEngine = new(_maxLevel, _quiet, _log);
                Data.DynDnsData dynDnsData = dataEngine.ReadDataFile(IpBufferPath(ipBuffer));

                if (dynDnsData == null)
                {
                    _log.WriteTrace(Log.TraceLevel.Warning, _maxLevel, "Fabric.Run", "Cannot read IP Buffer file. No zone records defined.", _quiet);
                    return false;
                }


                // Compare local IP with actual IP.
                // If equal, we don't need to do anything. return true.
                if (actualIp.Equals(dynDnsData.CurrentIp))
                {
                    string msg = $"Actual IP '{actualIp}' is equal to stored IP '{dynDnsData.CurrentIp}'. No need for updates.";
                    _log.WriteTrace(Log.TraceLevel.Success, _maxLevel, "Fabric.Run", msg, _quiet);
                    return true;
                }
                else
                {
                    string msg = $"Actual IP '{actualIp}' differs from stored IP '{dynDnsData.CurrentIp}'. Need to update DNS servers and local data file.";
                    _log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.Run", msg, _quiet);
                }


                // Update the AWS DNS server according with zone records read from zones.dat.

                Zone.ZoneEngine zoneEngine = new(_maxLevel, _quiet, _log);
                List<Zone.ZoneRecord> zoneRecords = zoneEngine.LoadZones(ZoneFilePath(zoneFilePath));

                if (zoneRecords == null || zoneRecords.Count == 0)
                {
                    _log.WriteTrace(Log.TraceLevel.Warning, _maxLevel, "Fabric.Run", "Cannot update DNS zones. No zone records defined.", _quiet);
                    return false;
                }

                foreach(var zoneRecord in zoneRecords)
                {
                    string msg = $"Updating zone '{zoneRecord.ZoneId}', record '{zoneRecord.RecordName.Trim()}' with IP '{actualIp}'.";
                    _log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.Run", msg, _quiet);
                    IpEngine ipEngine = new(_maxLevel, _quiet, _log);
                    ipEngine.ChangeResourceRecordSet(zoneRecord.ZoneId, zoneRecord.RecordName, actualIp, dynDnsData.CurrentIp, sharp);
                }


                //Update the local data file with actual IP .
                dynDnsData.CurrentIp = actualIp;
                dataEngine.WriteDataFile(ExecDirectory, dynDnsData);


                return true;
            }
            catch (Exception ex)
            {
                _log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.Run", $"Error running Engine: ", _quiet, ex);
                throw;
            }
        }

        private string ZoneFilePath(string argFilePath)
        {
            if (!string.IsNullOrWhiteSpace(argFilePath))
            {
                return argFilePath;
            }
            else
            {
                return Path.Combine(ExecDirectory, "zones.dat");
            }

        }

        private string IpBufferPath(string argFilePath)
        {
            if (!string.IsNullOrWhiteSpace(argFilePath))
            {
                return argFilePath;
            }
            else
            {
                return Path.Combine(ExecDirectory, "dyndns.dat");
            }
        }

        /// <summary>
        /// >Todo: Implement this!
        /// </summary>
        /// <param name="argFilePath"></param>
        /// <returns></returns>
        private string LogFilePath(string argFilePath)
        {
            if (!string.IsNullOrWhiteSpace(argFilePath))
            {
                return argFilePath;
            }
            else
            {
                return Path.Combine(Path.DirectorySeparatorChar.ToString(), "var", "log", "dyndns");
            }
        }

        private string ExecDirectory
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return "/opt/dyndns";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                }
                else
                {
                    throw new Exception("OS not supported.");
                }
            }
        }
    }
}
