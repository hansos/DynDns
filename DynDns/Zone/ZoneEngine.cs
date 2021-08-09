using DynDns.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace DynDns.Zone
{
    internal class ZoneEngine
    {
        private Log.TraceLevel _maxLevel;
        private const string _zoneFileName = "zones.dat";
        private const int _zoneIdLength = 21;

        public ZoneEngine(Log.TraceLevel maxLevel)
        {
            _maxLevel = maxLevel;
        }

        internal List<Zone.ZoneRecord> LoadZones(string execDir)
        {
            try
            {
                var fullPath = Path.Combine(execDir, _zoneFileName);

                // Return null if file is not fond.

                if (!File.Exists(fullPath))
                {
                    Log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.LoadZones", $"Zone File '{fullPath}' not found.");
                    return null;
                }

                // Load the zone file and return as a ZoneRecord list.

                Log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.LoadZones", $"Reading zones from '{fullPath}'...");
                List<Zone.ZoneRecord> washedLines = new();
                var lines = File.ReadAllLines(fullPath);
                foreach (var line in lines)
                {
                    if (line.Trim()[0] != '#' && line.Contains(';') && line.Length > 27)
                    {
                        var rec = line.Split(";", StringSplitOptions.RemoveEmptyEntries);
                        if (rec.Length != 2)
                        {
                            Log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.LoadZones", $"Zone record '{line}' is not valid. Should contain two values.");
                            return null;
                        }

                        if (rec[0].Length != _zoneIdLength)
                        {
                            Log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.LoadZones", $"Zone record '{line}' is not valid. Zone ID must be {_zoneIdLength} characters long.");
                            return null;
                        }

                        washedLines.Add(new Zone.ZoneRecord
                        {
                            ZoneId = rec[0].Trim(),
                            RecordName = rec[1].Trim()
                        });

                    }
                    else if (line.Trim()[0] != '#')
                    {
                        Log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.LoadZones", $"Zone record '{line}' is not valid.");
                        return null;
                    }
                }

                if (washedLines.Count == 0)
                {
                    Log.WriteTrace(Log.TraceLevel.Warning, _maxLevel, "Fabric.LoadZones", $"No Zone Records found.");
                    return null;
                }
                else
                {
                    Log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.LoadZones", $"Number of Zone Records loaded: {washedLines.Count} ");
                    return washedLines;
                }

            }
            catch (Exception ex)
            {
                Log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.LoadZones", $"Error loading zone file: ", ex);
                throw;
            }
        }

    }
}
