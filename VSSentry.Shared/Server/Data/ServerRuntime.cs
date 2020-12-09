using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{

    public class ServerRuntime
    {
        public string raw_description { get; set; }
        public string version { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }

}
