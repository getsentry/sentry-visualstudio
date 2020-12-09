using Microsoft.VisualStudio.Core.Imaging;
using Microsoft.VisualStudio.Language.CodeLens;
using Microsoft.VisualStudio.Language.CodeLens.Remoting;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VSSentry.Shared;
using VSSentry.Shared.Server;

namespace VSSentry
{
    public class SentryDataPoint : IAsyncCodeLensDataPoint
    {
        public readonly Guid id = Guid.NewGuid();
        private readonly SentryConnection _sentryConnection;
        private readonly CodeLensDescriptor _descriptor;
        private string query;

        public SentryDataPoint(SentryConnection sentryConnection, CodeLensDescriptor descriptor)
        {
            _sentryConnection = sentryConnection;
            _descriptor = descriptor;
            _sentryConnection.OptionsChanged += InvalidatedAsync;
        }

        private static readonly CodeLensDetailEntryCommand searchInSentryCommand = new CodeLensDetailEntryCommand
        {
            CommandSet = new Guid("FD3AA9D2-6CF8-46F4-879C-DF3C38C07B9C"),
            CommandId = 4129,
            CommandName = "Sentry.Search"
        };
        private static readonly CodeLensDetailEntryCommand openInSentryCommand = new CodeLensDetailEntryCommand
        {
            CommandSet = new Guid("FD3AA9D2-6CF8-46F4-879C-DF3C38C07B9C"),
            CommandId = 256,
            CommandName = "Sentry.Open"
        };

        public CodeLensDescriptor Descriptor => throw new NotImplementedException();

        public event AsyncEventHandler InvalidatedAsync;
        public List<SentryIssue> Data { get; set; }

        private static Regex _nameSpaceRegex = new Regex(@"namespace ([^{]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        public async Task<CodeLensDataPointDescriptor> GetDataAsync(CodeLensDescriptorContext descriptorContext, CancellationToken token)
        {
            Console.WriteLine(_descriptor.ElementDescription);

            if (!_sentryConnection.IsEnabled) return null;

            // TODO: Can do way better than this via using streams and only reading until the namespace is matched
            // it might reduce IO usage compared to reading the whole file
            var nsMatch = _nameSpaceRegex.Match(File.ReadAllText(_descriptor.FilePath));
            string ns = string.Empty;
            if (nsMatch.Success)
            {
                ns = nsMatch.Groups[1].Value.Trim();
            }
            var split = _descriptor.ElementDescription.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var @class = split.First().Trim();
            var method = split.Last().Trim();

            query = $"{ns}.{@class} in {method}";

            Logging.LogCL($"Sentry Query: {query}");
            Data = (await _sentryConnection.GetIssues($"{ns}.{@class}%20in%20{method}")).ToList();

            int errors = Data.Sum(x => int.TryParse(x.count, out var i) ? i : 0);
            return new CodeLensDataPointDescriptor()
            {
                IntValue = errors,
                Description = $"{errors} exception{(errors == 1 ? "" : "s")}",
                ImageId = errors == 0 ? default : new ImageId(new Guid("ae27a6b0-e345-4288-96df-5eaf394ee369"), 3175), // TODO: Replace with custom Sentry icon
                TooltipText = "Errors recorded in Sentry",
            };
        }

        public async Task<CodeLensDetailsDescriptor> GetDetailsAsync(CodeLensDescriptorContext descriptorContext, CancellationToken token)
        {
            return new CodeLensDetailsDescriptor
            {
                Headers = GetHeaders(),
                Entries = CreateEntries(Data),
                CustomData = new []{ new SentryIssues{ Issues = Data } },
                PaneNavigationCommands = new[] {
                    new CodeLensDetailPaneCommand {
                        CommandDisplayName = "Open in Sentry",
                        CommandId = searchInSentryCommand,
                        CommandArgs = new object[]{ new ProjectIdWithQuery{ ProjectId = _sentryConnection.ProjectId, Query = query } }
                    }
                },
            };
        }

        private IEnumerable<CodeLensDetailEntryDescriptor> CreateEntries(IEnumerable<SentryIssue> issues)
        {
            return issues.Select(
                issue => new CodeLensDetailEntryDescriptor()
                {
                    Fields = new List<CodeLensDetailEntryField>()
                    {
                            new CodeLensDetailEntryField()
                            {
                                Text = issue.lastSeen.ToString(@"MM\/dd\/yyyy", CultureInfo.CurrentCulture),
                            },
                            new CodeLensDetailEntryField()
                            {
                                Text = issue.count,
                            },
                            new CodeLensDetailEntryField()
                            {
                                Text = issue.title,
                            },
                    },
                    Tooltip = $"Open issue {issue.id} in Sentry",
                    NavigationCommand = openInSentryCommand,
                    NavigationCommandArgs = new List<object>() { new IssueReference { ProjectId = _sentryConnection.ProjectId, IssueId = issue.id } },
                });
        }

        private static IEnumerable<CodeLensDetailHeaderDescriptor> GetHeaders()
        {
            return new List<CodeLensDetailHeaderDescriptor>
            {
                new CodeLensDetailHeaderDescriptor
                {
                    DisplayName = "Last Seen",
                    Width = 85
                },
                new CodeLensDetailHeaderDescriptor
                {
                    DisplayName = "Times Seen",
                    Width = 85
                },
                new CodeLensDetailHeaderDescriptor
                {
                    DisplayName = "Message",
                    Width = 1
                }
            };
        }
    }
}
