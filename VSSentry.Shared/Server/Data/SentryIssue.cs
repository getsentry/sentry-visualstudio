using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VSSentry.Shared.Server.Data;

namespace VSSentry.Shared.Server
{
    [Serializable]
    public class SentryIssue
    {
        public string platform { get; set; }
        public Lifetime lifetime { get; set; }
        public int numComments { get; set; }
        public int userCount { get; set; }
        public Stats stats { get; set; }
        public string culprit { get; set; }
        public string title { get; set; }
        public string id { get; set; }
        public object assignedTo { get; set; }
        public Filtered filtered { get; set; }
        public string logger { get; set; }
        public string type { get; set; }
        public object[] annotations { get; set; }
        public Metadata metadata { get; set; }
        public string status { get; set; }
        public object subscriptionDetails { get; set; }
        public bool isPublic { get; set; }
        public bool hasSeen { get; set; }
        public string shortId { get; set; }
        public object shareId { get; set; }
        public DateTime firstSeen { get; set; }
        public string count { get; set; }
        public string permalink { get; set; }
        public string level { get; set; }
        public bool isSubscribed { get; set; }
        public bool isBookmarked { get; set; }
        public Project project { get; set; }
        public Statusdetails statusDetails { get; set; }
        public DateTime lastSeen { get; set; }
    }

    [Serializable]
    public class Lifetime
    {
        public string count { get; set; }
        public object stats { get; set; }
        public DateTime lastSeen { get; set; }
        public DateTime firstSeen { get; set; }
        public int userCount { get; set; }
    }

    [Serializable]
    public class Stats
    {
        [JsonProperty("30d")]
        public int[][] _30d { get; set; }

        [JsonProperty("24h")]
        public int[][] _24h { get; set; }

        [JsonIgnore]
        public Stat[] OneDayData => _24h?.Select(x => new Stat(x[0], x[1])).ToArray();

        [JsonIgnore]
        public Stat[] ThirtyDayChartData => _30d?.Select(x => new Stat(x[0], x[1])).ToArray();
        public class Stat
        {
            public DateTime Date { get; set; }
            public double Value { get; set; }
            public Stat(DateTime dt, double val)
            {
                Date = dt;
                Value = val;
            }
            public Stat(long unixTime, double val)
            {
                Date = DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
                Value = val;
            }
        }
    }

    [Serializable]
    public class Filtered
    {
        public string count { get; set; }
        public Stats stats { get; set; }
        public DateTime lastSeen { get; set; }
        public DateTime firstSeen { get; set; }
        public int userCount { get; set; }
    }

    [Serializable]
    public class Metadata
    {
        public string function { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public string filename { get; set; }
    }

    [Serializable]
    public class Statusdetails
    {
    }

}
