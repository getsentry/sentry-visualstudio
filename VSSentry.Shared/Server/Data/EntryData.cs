using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{

    public class EntryData
    {
        public string[][] cookies { get; set; }
        public object data { get; set; }
        public Dictionary<string, string> env { get; set; }
        public object excOmitted { get; set; }
        public string formatted { get; set; }
        public object fragment { get; set; }
        public bool hasSystemFrames { get; set; }
        public string[][] headers { get; set; }
        public object inferredContentType { get; set; }
        public string method { get; set; }
        public object[] query { get; set; }
        public string url { get; set; }
        public Value[] values { get; set; }
        public class Value
        {
            public object mechanism { get; set; }
            public string module { get; set; }
            public object rawStacktrace { get; set; }
            public Stacktrace stacktrace { get; set; }
            public int threadId { get; set; }
            public string type { get; set; }
            public string value { get; set; }
            public string message { get; set; }
            public string level { get; set; }
            public DateTime? timestamp { get; set; }
            public string category { get; set; }
        }
    }
}
