using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VSSentry.Shared.Options;
using VSSentry.Shared.Server.Data;

namespace VSSentry.Shared.Server
{
    public class SentryConnection
    {
        private static readonly Dictionary<Guid, SentryConnection> _projects = new Dictionary<Guid, SentryConnection>();
        private readonly HttpClient _httpClient = new HttpClient();
        public readonly Guid ProjectId;
        private SentryProjectOptions _options;
        public SentryConnection(Guid projectId, SentryProjectOptions options)
        {
            _options = options;
            ProjectId = projectId;
            ConfigureClient();
        }

        public event AsyncEventHandler OptionsChanged;

        public Uri ApiPath => new Uri(ServerUri, $"/api/0/organizations/{Organization}/issues/");
        public Uri BaseApiPath => new Uri(ServerUri, $"/api/0/issues/");
        public string SentryProject => _options.Project;
        public string Server => _options.ServerUrl;
        public string Organization => _options.Organization;
        public Uri ServerUri => new Uri(Server, UriKind.Absolute);
        public bool IsEnabled => _options != null;
        public static SentryConnection GetCurrent(Guid projectId)
        {
            SentryConnection connection;
            if (!_projects.TryGetValue(projectId, out connection))
            {
                connection = new SentryConnection(projectId, SentryProjectOptions.LoadOptions(projectId));
                _projects[projectId] = connection;
            }
            return connection;
        }

        public async Task<IEnumerable<SentryIssue>> GetIssues(string method)
        {
            if (_options == null)
            {
                return new SentryIssue[0];
            }
            var queryParams = $"limit=25&project={SentryProject}&query=%22{method}%22&shortIdLookup=1&statsPeriod=14d";

            var url = $"{ApiPath}?{queryParams}";
            Logging.LogCL($"Sending GET: {url}");
            var result = await _httpClient.GetStringAsync(url);
            var array = JsonConvert.DeserializeObject<SentryIssue[]>(result);
            return array;
        }


        public async Task<SentryIssueDetails> GetIssueDetails(string id)
        {
            if (_options == null)
            {
                return default;
            }

            var url = $"{BaseApiPath}{id}/";
            Logging.LogCL($"Sending GET: {url}");
            try
            {
                var result = await _httpClient.GetStringAsync(url);
                var details = JsonConvert.DeserializeObject<SentryIssueDetails>(result);
                details.Connection = this;
                return details;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Url: {url}");
                Debug.WriteLine(e.Message);
                throw;
            }
        }


        public async Task<SentryEvent> GetIssueLatestEvent(string id)
        {
            if (_options == null)
            {
                return default;
            }

            var url = $"{BaseApiPath}{id}/events/latest/";
            Logging.LogCL($"Sending GET: {url}");
            try
            {
                var result = await _httpClient.GetStringAsync(url);
                var details = JsonConvert.DeserializeObject<SentryEvent>(result);
                return details;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Url: {url}");
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<SentryEvent> GetIssueEvent(SentryIssueDetails issue, string eventId)
        {
            if (_options == null)
            {
                return default;
            }
            
            var url = new Uri(ServerUri, $"/api/0/projects/{Organization}/{issue.project.slug}/events/{eventId}/");
            Logging.LogCL($"Sending GET: {url}");
            try
            {
                var result = await _httpClient.GetStringAsync(url);
                var details = JsonConvert.DeserializeObject<SentryEvent>(result);
                return details;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Url: {url}");
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<SentryEventListItem[]> GetIssueEvents(string id)
        {
            if(_options == null)
            {
                return default;
            }

            var url = $"{BaseApiPath}{id}/events/?limit=50&query=";
            Logging.LogCL($"Sending GET: {url}");
            Logging.LogCL($"Sending GET: {url}");
            try
            {
                var result = await _httpClient.GetStringAsync(url);
                var details = JsonConvert.DeserializeObject<SentryEventListItem[]>(result);
                return details;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Url: {url}");
                Debug.WriteLine(e.Message);
                throw;
            }
        }


        public string GetUrl()
        {
            return Server;
        }

        public string GetUrlForIssue(string issueId)
        {
            return $"{Server}/organizations/{Organization}/issues/{issueId}/?project={SentryProject}";
        }

        public string GetUrlForSearch(string query)
        {
            return $"{Server}/organizations/{Organization}/issues/?project={SentryProject}&query={query}&statsPeriod=14d";
        }

        public async Task SaveSettingsAsync(SentryProjectOptions options)
        {
            options.SaveOptions(ProjectId);
            _options = options;
            ConfigureClient();
            if (OptionsChanged != null)
            {
                await OptionsChanged?.InvokeAsync(this, default);
            }
        }

        private void ConfigureClient()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        }

        private static string FindProjectPath(string filePath)
        {
            string directory = filePath;
            if (File.Exists(filePath))
            {
                directory = Directory.GetParent(filePath).FullName;
            }
            while (!Directory.GetFiles(directory).Any(x => x.EndsWith(".csproj")))
            {
                directory = Directory.GetParent(directory)?.FullName;
                if (directory == null)
                {
                    break;
                }
            }

            return directory;
        }
    }
}
