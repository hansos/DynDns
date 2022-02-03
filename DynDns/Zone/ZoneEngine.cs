using DynDns.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace DynDns.Zone
{
    internal class ZoneEngine
    {
        private Log.TraceLevel _maxLevel;
        private readonly bool _quiet;
        private const string _zoneFileName = "zones.dat";
        private const int _maxZoneIdLength = 32;
        private Log _log;
        public ZoneEngine(Log.TraceLevel maxLevel, bool quiet, Log log)
        {
            _maxLevel = maxLevel;
            _quiet = quiet;
            _log = log;
        }

        internal List<Zone.ZoneRecord> LoadZones(string zoneFilePath)
        {
            try
            {
                // Return null if file is not fond.

                if (!File.Exists(zoneFilePath))
                {
                    _log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.LoadZones", $"Zone File '{zoneFilePath}' not found.", _quiet);
                    return null;
                }

                // Load the zone file and return as a ZoneRecord list.

                _log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.LoadZones", $"Reading zones from '{zoneFilePath}'...", _quiet);
                List<Zone.ZoneRecord> washedLines = new();
                var lines = File.ReadAllLines(zoneFilePath);
                foreach (var line in lines)
                {
                    if (line.Trim()[0] != '#' && line.Contains(';') && line.Length > 27)
                    {
                        var rec = line.Split(";", StringSplitOptions.RemoveEmptyEntries);
                        if (rec.Length != 2)
                        {
                            _log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.LoadZones", $"Zone record '{line}' is not valid. Should contain two values.", _quiet);
                            return null;
                        }

                        if (rec[0].Length > _maxZoneIdLength)
                        {
                            _log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.LoadZones", $"Zone record '{line}' is not valid. Zone ID must be max {_maxZoneIdLength} characters long.", _quiet);
                            return null;
                        }

                        washedLines.Add(new Zone.ZoneRecord
                        {
                            ZoneId = rec[0].Trim(),
                            RecordName = NormalizeRecordName(rec[1]),
                        });

                    }
                    else if (line.Trim()[0] != '#')
                    {
                        _log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.LoadZones", $"Zone record '{line}' is not valid.", _quiet);
                        return null;
                    }
                }

                if (washedLines.Count == 0)
                {
                    _log.WriteTrace(Log.TraceLevel.Warning, _maxLevel, "Fabric.LoadZones", $"No Zone Records found.", _quiet);
                    return null;
                }
                else
                {
                    _log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "Fabric.LoadZones", $"Number of Zone Records loaded: {washedLines.Count} ", _quiet);
                    return washedLines;
                }

            }
            catch (Exception ex)
            {
                _log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "Fabric.LoadZones", $"Error loading zone file: ", _quiet, ex);
                throw;
            }
        }

        private string NormalizeRecordName(string recordName)
        {
            var s = recordName.Trim();
            if (!s.EndsWith('.'))
            {
                s = s + ".";
            }
            return s;
        }
    }
}
