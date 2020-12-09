using Microsoft.Win32;
using System;
using System.Linq;

namespace VSSentry.Shared.Options
{
    public class SentryProjectOptions
    {
        public string ServerUrl { get; set; }
        public string Project { get; set; }
        public string ApiKey { get; set; }
        public string Organization { get; set; }

        public static SentryProjectOptions LoadOptions(Guid projectId)
        {
            // Computer\HKEY_CURRENT_USER\SOFTWARE\Sentry\Visual Studio
            var extensionKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("Sentry").CreateSubKey("Visual Studio");

            var projectKey = extensionKey.CreateSubKey(projectId.ToString("D"));

            if (new[] { nameof(ServerUrl), nameof(Project), nameof(ApiKey) }.All(x => projectKey.GetValueNames().Contains(x)))
            {
                return new SentryProjectOptions()
                {
                    ServerUrl = projectKey.GetValue(nameof(ServerUrl)).ToString(),
                    Organization = projectKey.GetValue(nameof(Organization))?.ToString() ?? "sentry",
                    Project = projectKey.GetValue(nameof(Project)).ToString(),
                    ApiKey = projectKey.GetValue(nameof(ApiKey)).ToString(),
                };
            }
            return null;
        }

        public void SaveOptions(Guid projectId){
            // Computer\HKEY_CURRENT_USER\SOFTWARE\Sentry\Visual Studio
            var extensionKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("Sentry").CreateSubKey("Visual Studio");

            var projectKey = extensionKey.CreateSubKey(projectId.ToString("D"));
            projectKey.SetValue(nameof(ServerUrl), ServerUrl);
            projectKey.SetValue(nameof(Organization), Organization ?? "sentry");
            projectKey.SetValue(nameof(Project), Project);
            projectKey.SetValue(nameof(ApiKey), ApiKey);
        }

        public static void SaveOptions(Guid projectId, SentryProjectOptions options)
        {
            options.SaveOptions(projectId);
        }
    }
}
