using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{
    public class ServerOs
    {
        public string kernel_version { get; set; }
        public string name { get; set; }
        public string raw_description { get; set; }
        public string type { get; set; }
    }
}
