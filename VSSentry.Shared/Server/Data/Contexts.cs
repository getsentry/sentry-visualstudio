using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{

    public class Contexts
    {
        public ServerRuntime serverruntime { get; set; }
        public CurrentUICulture CurrentUICulture { get; set; }
        public CurrentCulture CurrentCulture { get; set; }

        [JsonProperty("client_os")]
        public ClientOs client_os { get; set; }
        public Device device { get; set; }

        [JsonProperty("client-os")]
        public ServerOs serveros { get; set; }
        public Browser browser { get; set; }
    }

}
