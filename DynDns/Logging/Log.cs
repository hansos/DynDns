using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns.Logging
{
    internal static class Log
    {

        internal enum TraceLevel
        {
            None = 0,
            Error = 1,
            Success = 2,
            Warning = 3,
            Trace = 4,
        }

        private static string destFolder = Path.Combine(Path.DirectorySeparatorChar.ToString(),"var", "log", "dyndns");
        
        private static string dailyFileName {
            get => $"dyndns_{DateTime.Now.ToString("yyyyMMdd")}.log";
        }

        private static string ipChangeFileName = "IpChanges.log";
        
        internal static void CreateDirectoryIfNeeded()
        {
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
        }

        internal static void LogIpChange(TraceLevel maxLevel, string oldIp, string newIp, bool quiet)
        {
            if(maxLevel < TraceLevel.Success)
            {
                return;
            }

            string message = $"IP changed from  {oldIp} to {newIp}";
            using (var f = new StreamWriter(Path.Combine(destFolder, ipChangeFileName), true))
            {
                var line = $"{DateTime.Now.ToString("yyy.MM.dd HH.mm.ss")} {message}";
                f.WriteLine(line);
            }
            if (!quiet)
            {
                Console.WriteLine($"{message}");
            }
        }

        internal static void WriteTrace(TraceLevel level, TraceLevel maxLevel, string method, string message, bool quiet, Exception exeption = null)
        {

            if (level > maxLevel)
            {
                return;
            }

            string ex = exeption != null ? exeption.Message : string.Empty;
            using (var f = new StreamWriter(Path.Combine(destFolder, dailyFileName),true))
            {
                var line = $"{DateTime.Now.ToString("HH.mm.ss")} {level.ToString().ToUpper(),-7} [{method}] {message} {ex}";
                f.WriteLine(line);
            }

            if (!quiet)
            {
                Console.WriteLine($"{message} {ex}");
            }
        }



    }
}
