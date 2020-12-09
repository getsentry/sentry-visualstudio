using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using VSSentry.Shared.Server;

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
    [Guid("853e0284-9f43-4692-8afc-3ef28117dc07")]
    public class SentryIssueDetailsWindow : ToolWindowPane
    {
        public readonly SentryIssueDetailsWindowControl ContentControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SentryIssueDetailsWindow"/> class.
        /// </summary>
        public SentryIssueDetailsWindow() : base(null)
        {
            this.Caption = "Sentry Details";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            ContentControl = new SentryIssueDetailsWindowControl();
            this.Content = ContentControl;
        }
    }
}
