using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns.Data
{
    internal class DataEngine
    {
        private const string _dataFileName = "dyndns.dat";
        private const string _keyCurrentIp = "currentip";

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
                Log.WriteLine(Log.TraceLevel.Success, "DataEngine.NewOrDefault", $"Replaced data file '{fullPath}'.");
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(Log.TraceLevel.Error, "DataEngine.NewOrDefault", $"Error loading data file:", ex);
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
                var fullPath = Path.Combine(dataFilePath, _dataFileName);
                DynDnsData dynDnsData = new();

                if (File.Exists(fullPath))
                {
                    var lines = File.ReadAllLines(fullPath);
                    foreach (var line in lines)
                    {
                        var lineParts = line.Split("=", 2);
                        if (lineParts[0].ToLower().Equals(_keyCurrentIp))
                        {
                            dynDnsData.CurrentIp = lineParts[1];
                        }
                    }
                    Log.WriteLine(Log.TraceLevel.Success, "DataEngine.NewOrDefault", $"Data file '{fullPath}' loaded.");
                    return dynDnsData;
                }
                else
                {
                    Log.WriteLine(Log.TraceLevel.Error, "DataEngine.NewOrDefault", $"Data file '{fullPath}' not found.");
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.WriteLine(Log.TraceLevel.Error, "DataEngine.NewOrDefault", $"Error loading data file:", ex);
                throw;
            }
        }

    }
}
