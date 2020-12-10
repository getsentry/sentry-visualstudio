using EnvDTE;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VSSentry.Shared.Options;
using Task = System.Threading.Tasks.Task;

namespace VSSentry.UI
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class SentryProjectSettingsWindowCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4129;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("dfc6d6e0-8b8c-47a0-8386-f88fa4a2f810");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="SentryProjectSettingsWindowCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private SentryProjectSettingsWindowCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static SentryProjectSettingsWindowCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in SentryProjectSettingsWindowCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new SentryProjectSettingsWindowCommand(package, commandService);
        }

        /// <summary>
        /// Shows the tool window when the menu item is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            this.package.JoinableTaskFactory.RunAsync(async delegate
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                //ToolWindowPane window = await this.package.ShowToolWindowAsync(typeof(SentryProjectSettingsWindow), 0, true, this.package.DisposalToken);
                var uiShell = await ServiceProvider.GetServiceAsync(typeof(SVsUIShell)) as IVsUIShell;
                if(uiShell == null) throw new InvalidCastException(nameof(uiShell));
                var window = new SentryProjectSettingsWindow(package);
                IntPtr hwnd;
                uiShell.GetDialogOwnerHwnd(out hwnd);
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                window.Width = 500;
                window.Height = 600;
                uiShell.EnableModeless(0);
                if ((null == window))
                {
                    throw new NotSupportedException("Cannot create tool window");
                }
                var projectWindow = (SentryProjectSettingsWindow)window;

                try
                {
                    WindowHelper.ShowModal(window, hwnd);
                }
                finally
                {
                    // This will take place after the window is closed.
                    uiShell.EnableModeless(1);
                }

                //var solution = await this.ServiceProvider.GetServiceAsync(typeof(IVsSolution)) as IVsSolution;

                //var hierarchies = GetProjectsInSolution(solution);

                //var guids = hierarchies.Select(x => new {
                //    hierarchy = x,
                //    guid = x.GetGuidProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_ProjectIDGuid, out var y) == VSConstants.S_OK ? y : Guid.Empty
                //}).ToList();


                //if(guids.Any(w => w.guid != Guid.Empty))
                //{
                //    var projectId = guids.First(x => x.guid != Guid.Empty).guid;

                //    if(projectId != null)
                //    {
                //        projectWindow.ProjectId = projectId;
                //    }
                //}


                //projectWindow.ProjectId = e.ToString();
            });
        }

        public static IEnumerable<IVsHierarchy> GetProjectsInSolution(IVsSolution solution)
        {
            return GetProjectsInSolution(solution, __VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION);
        }

        public static IEnumerable<IVsHierarchy> GetProjectsInSolution(IVsSolution solution, __VSENUMPROJFLAGS flags)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (solution == null)
                yield break;

            IEnumHierarchies enumHierarchies;
            var guid = Guid.Empty;
            solution.GetProjectEnum((uint)flags, ref guid, out enumHierarchies);
            if (enumHierarchies == null)
                yield break;

            IVsHierarchy[] hierarchy = new IVsHierarchy[1];
            uint fetched;
            while (enumHierarchies.Next(1, hierarchy, out fetched) == VSConstants.S_OK && fetched == 1)
            {
                if (hierarchy.Length > 0 && hierarchy[0] != null)
                    yield return hierarchy[0];
            }
        }
    }
}
