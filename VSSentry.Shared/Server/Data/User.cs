using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{

    public class User
    {
        public string username { get; set; }
        public string name { get; set; }
        public string ip_address { get; set; }
        public string email { get; set; }
        public object data { get; set; }
        public string id { get; set; }
    }

}
