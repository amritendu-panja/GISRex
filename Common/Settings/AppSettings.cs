using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Settings
{
    public class AppSettings
    {
        public Security Security { get; set; }
    }

    public class Security
    {
        public string KeyHeader { get; set; }
        public string ApiKey { get; set; }
    }
}
