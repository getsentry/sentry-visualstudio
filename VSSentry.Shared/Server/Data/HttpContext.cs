using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{

    public class HttpContext
    {
        public Property[] Properties { get; set; }
        
        public Dictionary<string, ValueContainer> PropertiesDictionary => Properties.ToDictionary(x => x.Name, x=>x.Value);
        public ValueContainer QueryString => Properties.FirstOrDefault(x => x.Name == "QueryString")?.Value;
        public ValueContainer Query => Properties.FirstOrDefault(x => x.Name == "Query")?.Value;
        public ValueContainer Headers => Properties.FirstOrDefault(x => x.Name == "Headers")?.Value;
    }
}
