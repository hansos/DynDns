using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns
{
    internal static class Log
    {

        internal enum TraceLevel
        {
            Success,
            Warning,
            Error,
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

        internal static void LogIpChange(string oldIp, string newIp)
        {
            using (var f = new StreamWriter(Path.Combine(destFolder, ipChangeFileName), true))
            {
                var line = $"{DateTime.Now.ToString("yyy.MM.dd HH.mm.ss")} IP changed from  {oldIp} to {newIp}";
                f.WriteLine(line);
            }
        }

            internal static void WriteTrace(TraceLevel level, string method, string message, Exception exeption = null)
        {
            using (var f = new StreamWriter(Path.Combine(destFolder, dailyFileName),true))
            {
                string ex = exeption != null ? exeption.Message : string.Empty;
                var line = $"{DateTime.Now.ToString("HH.mm.ss")} {level.ToString().ToUpper(),-7} [{method}] {message} {ex}";
                f.WriteLine(line);
            }
        }



    }
}
