using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{

    public class SentryEvent
    {
        public Context context { get; set; }
        public Contexts contexts { get; set; }
        public object crashFile { get; set; }
        public string culprit { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime dateReceived { get; set; }
        public object dist { get; set; }
        public Entry[] entries { get; set; }
        public object[] errors { get; set; }
        public string eventID { get; set; }
        public string[] fingerprints { get; set; }
        public string groupID { get; set; }
        public GroupingConfig groupingConfig { get; set; }
        public string id { get; set; }
        public string location { get; set; }
        public string message { get; set; }
        public Metadata metadata { get; set; }
        public string nextEventID { get; set; }
        public Dictionary<string, string> packages { get; set; }
        public string platform { get; set; }
        public string previousEventID { get; set; }
        public string projectID { get; set; }
        public Release release { get; set; }
        public Sdk sdk { get; set; }
        public object[] sdkUpdates { get; set; }
        public int size { get; set; }
        public Tag[] tags { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public User user { get; set; }
        public object userReport { get; set; }
        public Stacktrace Stacktrace => Array.Find(entries, x => x.type == "exception")?.data.values?.FirstOrDefault(x => x.stacktrace != null).stacktrace;
        public bool HasUserData => user != null && contexts?.client_os != null;

        public class Tag
        {
            public string key { get; set; }
            public string query { get; set; }
            public string value { get; set; }
        }
    }
}
