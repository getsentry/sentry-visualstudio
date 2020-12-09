using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{

    public class ValueContainer
    {
        public Dictionary<string, string> Elements { get; set; }
        public object Value { get; set; }
    }
}
