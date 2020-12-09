using System;
using System.Collections.Generic;

namespace VSSentry.Shared.Server
{
    [Serializable]
    public class SentryIssues
    {
        public List<SentryIssue> Issues { get; set; }
    }

}
