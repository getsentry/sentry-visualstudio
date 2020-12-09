using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSSentry.Shared.CommandParameters
{
    public class ProjectIdWithQuery
    {
        public Guid ProjectId { get; set; }
        public string Query { get; set; }
    }
}
