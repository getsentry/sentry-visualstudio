using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{

    public class Device
    {
        public string timezone { get; set; }
        public string timezone_display_name { get; set; }
        public string type { get; set; }
    }
}
