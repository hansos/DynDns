using DynDns.ConsoleIo;
using DynDns.Logging;
using System;
using System.Diagnostics;
using System.Reflection;

namespace DynDns
{
    class Program
    {

        /// <summary>
        /// Program entry point. If command line arguments exists, 
        /// Calls the command line argument interpreter, If not, output a message on console and exit.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Log.TraceLevel maxLevel = Log.TraceLevel.Trace;
            try
            {
                if(args.Length == 0)
                {
                    Output.MissingArgumentText();
                    new Log(null).WriteTrace(Log.TraceLevel.Error, maxLevel, "Fabric.Fabric", $"No command line argument defined.", false);
                }
                else
                {
                    Interpret.InterpretArguments(args, maxLevel);
                }

            }
            catch (Exception ex)
            {
                new Log(null).WriteTrace(Log.TraceLevel.Error, maxLevel, "Fabric.Fabric", $"Program completed with error(s):",false, ex);
            }
        }
    }
}
