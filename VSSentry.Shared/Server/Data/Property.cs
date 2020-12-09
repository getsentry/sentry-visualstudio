using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{

    public class Property
    {
        public string Name { get; set; }
        public ValueContainer Value { get; set; }
    }
}
