using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns.Zone
{
    internal class ZoneEngine
    {
        private const string _zoneFileName = "zones.dat";
        private const int _zoneIdLength = 21;

        internal List<Zone.ZoneRecord> LoadZones(string execDir)
        {
            try
            {
                var fullPath = Path.Combine(execDir, _zoneFileName);

                // Return null if file is not fond.

                if (!File.Exists(fullPath))
                {
                    Log.WriteLine(Log.TraceLevel.Error, "Fabric.LoadZones", $"Zone File '{fullPath}' not found.");
                    return null;
                }

                // Load the zone file and return as a ZoneRecord list.

                Log.WriteLine(Log.TraceLevel.Success, "Fabric.LoadZones", $"Reading zones from '{fullPath}'...");
                List<Zone.ZoneRecord> washedLines = new();
                var lines = File.ReadAllLines(fullPath);
                foreach (var line in lines)
                {
                    if (line.Trim()[0] != '#' && line.Contains(';') && line.Length > 27)
                    {
                        var rec = line.Split(";", StringSplitOptions.RemoveEmptyEntries);
                        if (rec.Length != 2)
                        {
                            Log.WriteLine(Log.TraceLevel.Error, "Fabric.LoadZones", $"Zone record '{line}' is not valid. Should contain two values.");
                            return null;
                        }

                        if (rec[0].Length != _zoneIdLength)
                        {
                            Log.WriteLine(Log.TraceLevel.Error, "Fabric.LoadZones", $"Zone record '{line}' is not valid. Zone ID must be {_zoneIdLength} characters long.");
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
                        Log.WriteLine(Log.TraceLevel.Error, "Fabric.LoadZones", $"Zone record '{line}' is not valid.");
                        return null;
                    }
                }

                if (washedLines.Count == 0)
                {
                    Log.WriteLine(Log.TraceLevel.Warning, "Fabric.LoadZones", $"No Zone Records found.");
                    return null;
                }
                else
                {
                    Log.WriteLine(Log.TraceLevel.Success, "Fabric.LoadZones", $"Number of Zone Records loaded: {washedLines.Count} ");
                    return washedLines;
                }

            }
            catch (Exception ex)
            {
                Log.WriteLine(Log.TraceLevel.Error, "Fabric.LoadZones", $"Error loading zone file: ", ex);
                throw;
            }
        }

    }
}
