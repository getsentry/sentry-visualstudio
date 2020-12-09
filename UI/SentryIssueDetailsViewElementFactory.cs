using System;
using System.ComponentModel.Composition;
using System.Windows;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using VSSentry.Shared.Server;

namespace VSSentry.UI
{
    [Export(typeof(IViewElementFactory))]
    [Name("Sentry Issue details view element factory")]
    [TypeConversion(from: typeof(SentryIssues), to: typeof(UIElement))]
    [Order]
    internal class ViewElementFactory : IViewElementFactory
    {
        public TView CreateViewElement<TView>(ITextView textView, object model) where TView : class
            => new SentryIssueDetails{ DataContext = (SentryIssues)model } as TView;
    }
}
