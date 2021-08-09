using DynDns.Logging;
using System;
using System.IO;
using System.Text;

namespace DynDns.Data
{
    internal class DataEngine
    {
        private const string _dataFileName = "dyndns.dat";
        private const string _keyCurrentIp = "currentip";
        private Log.TraceLevel _maxLevel;
        private readonly bool _quiet;
        private Log _log;

        public DataEngine(Log.TraceLevel maxLevel, bool quiet, Log log)
        {
            _maxLevel = maxLevel;
            _quiet = quiet;
            _log = log;
        }

        /// <summary>
        /// Overwrite an existing dyndns.dat file with the content of a DynDnsData object.
        /// The data file will always be overwritten.
        /// </summary>
        /// <param name="dataFilePath">Path to </param>
        /// <param name="dynDnsData">An initialzed DynDnsData object.</param>
        /// <returns>True of the file were written.</returns>
        internal bool WriteDataFile(string dataFilePath, DynDnsData dynDnsData)
        {
            try
            {
                var fullPath = Path.Combine(dataFilePath, _dataFileName);

                StringBuilder sb = new();
                sb.AppendLine($"{_keyCurrentIp}={dynDnsData.CurrentIp}");
                File.WriteAllText(fullPath, sb.ToString());
                _log.WriteTrace(Log.TraceLevel.Trace,_maxLevel, "DataEngine.NewOrDefault", $"Replaced data file '{fullPath}'.", _quiet);
                return true;
            }
            catch (Exception ex)
            {
                _log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "DataEngine.NewOrDefault", $"Error loading data file:", _quiet, ex);
                throw;
            }
        }


        /// <summary>
        /// Open an existing data file and add it's content to a DynDns object instance.
        /// </summary>
        /// <param name="dataFilePath">Where to look for data file.</param>
        /// <returns></returns>
        internal DynDnsData ReadDataFile(string dataFilePath)
        {
            try
            {
                DynDnsData dynDnsData = new();

                if (File.Exists(dataFilePath))
                {
                    var lines = File.ReadAllLines(dataFilePath);
                    foreach (var line in lines)
                    {
                        var lineParts = line.Split("=", 2);
                        if (lineParts[0].ToLower().Equals(_keyCurrentIp))
                        {
                            dynDnsData.CurrentIp = lineParts[1];
                        }
                    }
                    _log.WriteTrace(Log.TraceLevel.Trace, _maxLevel, "DataEngine.NewOrDefault", $"Data file '{dataFilePath}' loaded.", _quiet);
                    return dynDnsData;
                }
                else
                {
                    _log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "DataEngine.NewOrDefault", $"Data file '{dataFilePath}' not found.", _quiet);
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.WriteTrace(Log.TraceLevel.Error, _maxLevel, "DataEngine.NewOrDefault", $"Error loading data file:", _quiet, ex);
                throw;
            }
        }

    }
}
