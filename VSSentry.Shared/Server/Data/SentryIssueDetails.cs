using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VSSentry.Shared.Server.Data
{
    [Serializable]
    public class SentryIssueDetails
    {
        public SeenBy[] seenBy { get; set; }
        public object[] pluginIssues { get; set; }
        public string platform { get; set; }
        public int userReportCount { get; set; }
        public int numComments { get; set; }
        public int userCount { get; set; }
        public Stats stats { get; set; }
        public string culprit { get; set; }
        public string title { get; set; }
        public string id { get; set; }
        public object assignedTo { get; set; }
        public object[] participants { get; set; }
        public string logger { get; set; }
        public string type { get; set; }
        public object[] annotations { get; set; }
        public Metadata metadata { get; set; }
        public string status { get; set; }
        public object[] pluginActions { get; set; }
        public Tag[] tags { get; set; }
        public object subscriptionDetails { get; set; }
        public bool isPublic { get; set; }
        public bool hasSeen { get; set; }
        public Release firstRelease { get; set; }
        public string shortId { get; set; }
        public object shareId { get; set; }
        public DateTime firstSeen { get; set; }
        public string count { get; set; }
        public string permalink { get; set; }
        public string level { get; set; }
        public bool isSubscribed { get; set; }
        public object[] pluginContexts { get; set; }
        public bool isBookmarked { get; set; }
        public Project project { get; set; }
        public Release lastRelease { get; set; }
        public Activity[] activity { get; set; }
        public Statusdetails statusDetails { get; set; }
        public DateTime lastSeen { get; set; }
        public ICommand OpenInBrowserCommand { get; set; }
        public SentryConnection Connection { get; internal set; }

        public SentryIssueDetails()
        {
             OpenInBrowserCommand = new DelegateCommand((_) => OpenInBrowser());
        }

        public void OpenInBrowserHander(object sender, EventArgs args)
        {
            OpenInBrowser();
        }

        public void OpenInBrowser()
        {
            var url = Connection.GetUrlForIssue(id);
            Process.Start(url);
        }
    }

    [Serializable]
    public class Release
    {
        public string status { get; set; }
        public object dateReleased { get; set; }
        public int newGroups { get; set; }
        public int commitCount { get; set; }
        public object url { get; set; }
        public Data data { get; set; }
        public object lastDeploy { get; set; }
        public int deployCount { get; set; }
        public DateTime? dateCreated { get; set; }
        public DateTime? lastEvent { get; set; }
        public string version { get; set; }
        public DateTime? firstEvent { get; set; }
        public object lastCommit { get; set; }
        public string shortVersion { get; set; }
        public object[] authors { get; set; }
        public object owner { get; set; }
        public Versioninfo versionInfo { get; set; }
        public object _ref { get; set; }
        public Project[] projects { get; set; }

        [Serializable]
        public class Data
        {
            public string version { get; set; }
            public int age { get; set; }
        }
    }

    [Serializable]
    public class Versioninfo
    {
        public object buildHash { get; set; }
        public Version version { get; set; }
        public string description { get; set; }
        public object package { get; set; }
    }

    [Serializable]
    public class Version
    {
        public string raw { get; set; }
    }

    [Serializable]
    public class Project
    {
        public string name { get; set; }
        public string platform { get; set; }
        public string slug { get; set; }
        public string[] platforms { get; set; }
        public bool hasHealthData { get; set; }
        public int newGroups { get; set; }
        public int id { get; set; }
    }

    [Serializable]
    public class SeenBy
    {
        public string username { get; set; }
        public DateTime lastLogin { get; set; }
        public bool isSuperuser { get; set; }
        public Email[] emails { get; set; }
        public bool isManaged { get; set; }
        public Experiments experiments { get; set; }
        public DateTime lastActive { get; set; }
        public bool isStaff { get; set; }
        public object[] identities { get; set; }
        public string id { get; set; }
        public bool isActive { get; set; }
        public bool has2fa { get; set; }
        public string name { get; set; }
        public string avatarUrl { get; set; }
        public DateTime dateJoined { get; set; }
        public Options options { get; set; }
        public Flags flags { get; set; }
        public Avatar avatar { get; set; }
        public bool hasPasswordAuth { get; set; }
        public string email { get; set; }
        public DateTime lastSeen { get; set; }
    }

    [Serializable]
    public class Experiments
    {
    }

    [Serializable]
    public class Options
    {
        public string timezone { get; set; }
        public int stacktraceOrder { get; set; }
        public string language { get; set; }
        public bool clock24Hours { get; set; }
    }

    [Serializable]
    public class Flags
    {
        public bool newsletter_consent_prompt { get; set; }
    }

    [Serializable]
    public class Avatar
    {
        public string avatarUuid { get; set; }
        public string avatarType { get; set; }
    }

    [Serializable]
    public class Email
    {
        public bool is_verified { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

    [Serializable]
    public class Tag
    {
        public int totalValues { get; set; }
        public string name { get; set; }
        public string key { get; set; }
    }

    [Serializable]
    public class Activity
    {
        public Release.Data data { get; set; }
        public DateTime dateCreated { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public User user { get; set; }
    }

}
