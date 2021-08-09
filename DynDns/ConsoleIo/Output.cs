using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DynDns.ConsoleIo
{
    static internal class Output
    {

        static internal void MissingArgumentText()
        {
            StringBuilder sb = new();
            sb.AppendLine("dyndns: missing command line arguments");
            sb.AppendLine("Try 'dyndns --help' for more information.");
            Console.Write(sb.ToString());
        }

        static internal void VersionText()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            StringBuilder sb = new();
            sb.AppendLine($"dyndns (AWS R53 zone record updater) {fileVersionInfo.ProductVersion}");
            sb.AppendLine(fileVersionInfo.LegalCopyright);
            sb.AppendLine("Licensed under the BSD-3-Clause License or later. See <https://github.com/hansos/DynDns/blob/master/LICENSE.txt>.");
            sb.AppendLine("This is free software: You are free to change and redistribute it under the conditions described in the licence.");
            sb.AppendLine("There is NO WARRANTY, to the extent permitted by law.");
            sb.AppendLine();
            sb.AppendLine("Written by Hans Olav Sorteberg.");
            Console.Write(sb.ToString());
        }

        static internal void HelpText()
        {
            StringBuilder sb = new();
            sb.AppendLine($"Usage: dyndns OPTION(s)");
            sb.AppendLine("Updates one or several AWS R53 Zone Records to the conputer's public IP address.");
            sb.AppendLine("To run this tool, you need an active AWS account and R53 DNS records for involved domains.");
            sb.AppendLine("The tool is connecting to AWS using credentials stored on the computer using the AWS console tool.");
            sb.AppendLine();
            sb.AppendLine("Unless otherwise specified, the zones are read from the 'zones.dat' data files located together with the program file.");
            sb.AppendLine();
            sb.AppendLine("Mandatory arguments to long options are mandatory for short options too.");
            sb.AppendLine();
            sb.AppendLine("      --help                Display this help and exit.");
            sb.AppendLine("      --version             Output version information and exit.");
            sb.AppendLine("  -q  --quiet               Supress all output to console.");
            sb.AppendLine("  -r  --run=[PATH]          Run the DNS update engine. Unless PATH to a data file is spesified,");
            sb.AppendLine("                              zones are read from the 'zones.dat' data files located together with the program file.");
            sb.AppendLine("  -t  --trace-level=LEVEL   Set trace level for trace file. 0=nothing, 4=full trace. Values and codes accepted.");
            sb.AppendLine("      --test-run            Run the DNS update engine, bot don't write the IP address to the DNS zone record.");
            sb.AppendLine("  -l  --log-path=PATH       Path to the log directory. If not submitted, log files are created in '/var/log/dyndns'.");
            sb.AppendLine();
            sb.AppendLine("Trace levels:");
            sb.AppendLine("  0    NONE                 No trace.");
            sb.AppendLine("  1    ERR                  Error messages only.");
            sb.AppendLine("  2    SUCC                 Error and success messages only.");
            sb.AppendLine("  3    WARN                 Warnings, errors and success messages.");
            sb.AppendLine("  4    TRACE                Full trace.");
            sb.AppendLine();
            sb.AppendLine("See <https://github.com/hansos/DynDns/blob/master/readme.md> for more information.");
            Console.Write(sb.ToString());
        }

       
    }
}
