﻿using DynDns.ConsoleIo;
using DynDns.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns
{
    internal static class Interpret
    {

        internal static void InterpretArguments(string[] args, Log.TraceLevel maxLevel)
        {

            Log.TraceLevel level = maxLevel;
            bool quiet = false;
            bool run = false;
            bool testrun = false;
            string ipBuffer = null;
            string logFilePath = null;
            string zoneFilePath = null;
            string url = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("--help"))
                {
                    Output.HelpText();
                    return;
                }
                else if (args[i].Equals("--version"))
                {
                    Output.VersionText();
                    return;
                }
                else
                {
                    if(args[i].Equals("-q") || args[i].Equals("--quiet"))
                    {
                        quiet = true;
                    }
                    else if (args[i].StartsWith("-r") || args[i].StartsWith("--run"))
                    {
                        run = true;
                        var splitted = args[i].Split("=", StringSplitOptions.RemoveEmptyEntries);
                        if (splitted.Length == 2)
                        {
                            zoneFilePath = splitted[1].Trim();
                        }
                    }
                    else if (args[i].StartsWith("-u") || args[i].StartsWith("--url"))
                    {
                        var splitted = args[i].Split("=", StringSplitOptions.RemoveEmptyEntries);
                        if (splitted.Length == 2)
                        {
                            url = splitted[1].Trim();
                        }
                    }
                    else if (args[i].StartsWith("--test-run"))
                    {
                        testrun = true;
                        var splitted = args[i].Split("=", StringSplitOptions.RemoveEmptyEntries);
                        if (splitted.Length == 2)
                        {
                            zoneFilePath = splitted[1].Trim();
                        }
                    }
                    else if (args[i].StartsWith("-t") || args[i].StartsWith("--trace-level"))
                    {
                        var splitted = args[i].Split("=", StringSplitOptions.RemoveEmptyEntries);
                        if (splitted.Length != 2)
                        {
                            Output.UnrecognizedOptionText(args[i]);
                            return;
                        }
                        if (splitted[1].Trim().Equals("0") || splitted[1].Trim().Equals("NONE"))
                        {
                            level = Log.TraceLevel.None;
                        }
                        else if (splitted[1].Trim().Equals("1") || splitted[1].Trim().StartsWith("ERR"))
                        {
                            level = Log.TraceLevel.Error;
                        }
                        else if (splitted[1].Trim().Equals("2") || splitted[1].Trim().StartsWith("SUCC"))
                        {
                            level = Log.TraceLevel.Success;
                        }
                        else if (splitted[1].Trim().Equals("3") || splitted[1].Trim().Equals("WARN"))
                        {
                            level = Log.TraceLevel.Warning;
                        }
                        else if (splitted[1].Trim().Equals("4") || splitted[1].Trim().Equals("TRACE"))
                        {
                            level = Log.TraceLevel.Trace;
                        }
                        else
                        {
                            Output.UnrecognizedOptionText(args[i]);
                            return;
                        }

                    }
                    else if (args[i].Equals("-l") || args[i].Equals("--log-path"))
                    {
                        var splitted = args[i].Split("=", StringSplitOptions.RemoveEmptyEntries);
                        if (splitted.Length != 2)
                        {
                            Output.UnrecognizedOptionText(args[i]);
                            return;
                        }
                        logFilePath = splitted[1].Trim();
                    }
                    else if (args[i].StartsWith("-i") || args[i].StartsWith("--ip-buffer-path"))
                    {
                        var splitted = args[i].Split("=", StringSplitOptions.RemoveEmptyEntries);
                        if (splitted.Length != 2)
                        {
                            Output.UnrecognizedOptionText(args[i]);
                            return;
                        }
                        ipBuffer = splitted[1].Trim();
                    }
                }
            }

            if (testrun && run)
            {
                Output.ErrorText("Both --run and --test-run are specified. These arguments cannot be used in combination.");
                return;
            }

            if (!testrun && !run)
            {
                Output.ErrorText("One of the --run and --test-run arguments must be specified.");
                return;
            }

            var result = new Fabric(level, quiet, logFilePath).Run(url, run, ipBuffer, zoneFilePath );


        }
    }
}
