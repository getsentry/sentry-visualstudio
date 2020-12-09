using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using VSSentry.Shared;
using IServiceProvider = System.IServiceProvider;
using Task = System.Threading.Tasks.Task;

namespace VSSentry
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("Sentry CodeLens", "CodeLens integration that pulls data from sentry.io (hosted or on-premises) to display a quick view of exceptions over methods.", "1.0.0.0")]
    [Guid(GuidAndCmdID.PackageGuidString)]
    [ProvideBindingPath]
    public sealed class VSSentryPackage : AsyncPackage
    {

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            try
            {
                // When initialized asynchronously, the current thread may be a background thread at this point.
                // Do any initialization that requires the UI thread after switching to the UI thread.
                await base.InitializeAsync(cancellationToken, progress);
                await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

                await SentryProjectSettingsWindowCommand.InitializeAsync(this).ConfigureAwait(false);
                await OpenIssueInSentryCommand.InitializeAsync(this).ConfigureAwait(false);
                await SearchInSentryCommand.InitializeAsync(this).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logging.LogVS(ex);
                throw;
            }
        }

        #endregion
    }
}
