using DynDns.ConsoleIo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns
{
    internal static class Interpret
    {
        internal static void InterpretArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower().Equals("--help"))
                {
                    Output.HelpText();
                }
                else if (args[i].ToLower().Equals("--version"))
                {
                    Output.VersionText();
                }
                else
                {

                }
            }
        }
    }
}
