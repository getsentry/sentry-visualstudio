using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using VSSentry.Shared.Options;
using VSSentry.Shared.Server;

namespace VSSentry.UI
{
    /// <summary>
    /// Interaction logic for SentryProjectSettingsWindowControl.
    /// </summary>
    public partial class SentryProjectSettingsWindowControl : UserControl
    {
        public Guid ProjectId { get; set; }

        private string _vsProjectName;
        public string VSProjectName
        {
            get => _vsProjectName;
            set
            {
                _vsProjectName= value;
                ProjectName.Text = value;
            }
        }
        public bool IsLoading
        {
            set
            {
                if (value)
                {
                    Loader.Visibility = Visibility.Visible;
                    ContentContainer.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Loader.Visibility = Visibility.Collapsed;
                    ContentContainer.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SentryProjectSettingsWindowControl"/> class.
        /// </summary>
        public SentryProjectSettingsWindowControl()
        {
            this.InitializeComponent();
        }

        public event EventHandler Close;

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var serverUrl = ServerUrl.Text;
            var projectId = SentryProjectID.Text;
            var apiKey = ApiKey.Password;
            var org = Organization.Text;

            var connection = SentryConnection.GetCurrent(ProjectId);
            await connection.SaveSettingsAsync(new SentryProjectOptions
            {
                ServerUrl = serverUrl,
                ApiKey = apiKey,
                Project = projectId,
                Organization = org
            });

            Close?.Invoke(this, e);
        }
    }
}
