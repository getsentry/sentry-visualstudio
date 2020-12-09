using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{
    public class Stacktrace
    {
        public Frame[] frames { get; set; }
        public object framesOmitted { get; set; }
        public bool hasSystemFrames { get; set; }
        public object registers { get; set; }
    }
}
