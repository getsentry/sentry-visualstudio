using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSSentry.Shared.CommandParameters
{
    public class IssueReference
    {
        public Guid ProjectId { get; set; }
        public string IssueId { get; set; }
    }
}
