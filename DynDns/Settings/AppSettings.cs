using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynDns.Settings
{
    public class AppSettings
    {
        public AwsSettings AwsSettings { get; set; } = new();
    }
}
