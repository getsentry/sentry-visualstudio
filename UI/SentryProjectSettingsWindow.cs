using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;
using VSSentry.Shared.Options;

namespace VSSentry.UI
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("dd08d6db-b9de-4cb0-ae8f-2441eb34beb9")]
    public class SentryProjectSettingsWindow : DialogWindow
    {
        private SentryProjectSettingsWindowControl _control;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SentryProjectSettingsWindow"/> class.
        /// </summary>
        public SentryProjectSettingsWindow(IServiceProvider serviceProvider) : base()
        {
            base.Title= "Sentry Project Settings";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            _control = new SentryProjectSettingsWindowControl();
            this.Content = _control;
            _control.Close += Close;
            this._serviceProvider = serviceProvider;
            _ = LoadAsync(_serviceProvider);
        }        

        public async System.Threading.Tasks.Task LoadAsync(IServiceProvider serviceProvider)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            IsLoading = true;
            var uiHierarchy = VsShellUtilities.GetUIHierarchyWindow(serviceProvider, new Guid(ToolWindowGuids80.SolutionExplorer));
            var uiHierarchy2 = uiHierarchy as IVsUIHierarchyWindow2;
            uiHierarchy2.GetCurrentSelection(out var hierarchy, out var itemId, out var multiSelect);
            hierarchy.GetGuidProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_ProjectIDGuid, out var projectId);
            hierarchy.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_ProjectName, out var projectName);



            if(hierarchy != null)
            {
                ProjectId = projectId;
                VSProjectName = projectName.ToString();

                var settings = SentryProjectOptions.LoadOptions(projectId);
                if(settings != null)
                {
                    ServerUrl = settings.ServerUrl;
                    SentryProject = settings.Project;
                    SentryApiKey = settings.ApiKey;
                    Organization = settings.Organization;
                }
            }
            IsLoading = false;
        }

        public void Close(object sender, EventArgs e)
        {
            base.Close();
        }

        public Guid ProjectId
        {
            get => _control.ProjectId;
            set => _control.ProjectId = value;
        }

        public string VSProjectName
        {
            get => _control.VSProjectName;
            set => _control.VSProjectName = value;
        }

        public string Organization
        {
            set => _control.Organization.Text = value;
        }

        public bool IsLoading
        {
            set => _control.IsLoading = value;
        }
        public string ServerUrl
        {
            set
            {
                _control.ServerUrl.Text = value;
            }
        }
        public string SentryProject
        {
            set
            {
                _control.SentryProjectID.Text = value;
            }
        }
        public string SentryApiKey
        {
            set
            {
                _control.ApiKey.Password = value;
            }
        }
    }
}
