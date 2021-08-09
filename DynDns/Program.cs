using DynDns.Logging;
using System;
using System.Diagnostics;
using System.Reflection;

namespace DynDns
{
    class Program
    {

        static void Main(string[] args)
        {
            Log.TraceLevel maxLevel = Log.TraceLevel.Trace;
            bool quietMode = false;
            bool doExecute = true;
            bool showMenu = false;
            try
            {

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].ToLower().Equals("-t"))
                    {
                        maxLevel = (Log.TraceLevel)Enum.Parse(typeof(Log.TraceLevel), args[++i]);
                    }
                    else if (args[i].ToLower().Equals("-q"))
                    {
                        quietMode = true;
                    }
                    else if (args[i].ToLower().Equals("-m"))
                    {
                        doExecute = false;
                        showMenu = true;
                    }

                }

                if(!quietMode)
                {
                    ShowAppDescription();
                    if (showMenu)
                    {
                        ShowMenu();
                    }

                }

                if (doExecute)
                {
                    var result = new Fabric(maxLevel).Run(quietMode);
                }

            }
            catch (Exception ex)
            {
                Log.WriteTrace(Log.TraceLevel.Error, maxLevel, "Fabric.Fabric", $"Program completed with error(s):", ex);
            }


        }

        private static void ShowMenu()
        {
            throw new NotImplementedException();
        }

        private static void ShowAppDescription()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Console.WriteLine($"EsoGame Dynamic DNS Updater version {fileVersionInfo.ProductVersion}");
            Console.WriteLine("Copyright (c) 2021 hans.olav@sorteberg.com, all rights reserved.");
        }

    }
}
