using System;
using System.Collections.Generic;

namespace VSSentry.Shared.Server.Data
{
    [Serializable]
    public class SentryIssues
    {
        public List<SentryIssue> Issues { get; set; }
    }

}
