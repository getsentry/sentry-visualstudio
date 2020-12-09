using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VSSentry.Shared.Server.Data
{
    public class SentryEventListItem
    {
        public string eventID { get; set; }
        public Tag[] tags { get; set; }
        public string projectID { get; set; }
        public DateTime dateCreated { get; set; }
        public User user { get; set; }
        public string message { get; set; }
        public string id { get; set; }
        public string culprit { get; set; }
        public string title { get; set; }
        public string platform { get; set; }
        public string location { get; set; }
        public object crashFile { get; set; }
        public string eventtype { get; set; }
        public string groupID { get; set; }
        public ICommand OpenIssueCommand { get; set; }
    }
}
