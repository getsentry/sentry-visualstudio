using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{

    public class Context
    {
        public string Application { get; set; }
        public string ConnectionId { get; set; }
        public string Environment { get; set; }
        public Eventid EventId { get; set; }
        public HttpContext HttpContext { get; set; }
        public string RequestId { get; set; }
        public string RequestPath { get; set; }
        public string SourceContext { get; set; }
    }
}
