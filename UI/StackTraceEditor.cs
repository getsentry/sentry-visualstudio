using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ICSharpCode.AvalonEdit;
using Microsoft.VisualStudio.LanguageServices;
using VSSentry.Generators;
using VSSentry.Helpers;
using VSSentry.Models;

namespace VSSentry.UI
{
    class StackTraceEditor : TextEditor
    {
        public static readonly DependencyProperty StacktraceProperty = DependencyProperty.Register("Stacktrace", typeof(string), typeof(StackTraceEditor),
        new FrameworkPropertyMetadata(
            string.Empty,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            new PropertyChangedCallback(OnStacktraceChangedAsync)
            //new CoerceValueCallback(CoerceCurrentReading)
        ));

        public string Stacktrace
        {
            get { return (string)GetValue(StacktraceProperty); }
            set { SetValue(StacktraceProperty, value); }
        }

        public StackTracesViewModel ViewModel { get; }

        public StackTraceEditor()
        {
            TextArea.TextView.ElementGenerators.Add(new FileLinkElementGenerator(this));
            TextArea.TextView.ElementGenerators.Add(new MemberLinkElementGenerator(this));
            ViewModel = new StackTracesViewModel();
            ViewModel.AddStackTrace("");
            DataContext = ViewModel;
            EnvDteHelper.ViewModel = ViewModel;
            var binding = new Binding("Document");
            binding.Source = ViewModel.StackTraces[0];
            SetBinding(DocumentProperty, binding);
            
        }

        private static async void OnStacktraceChangedAsync(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is StackTraceEditor editor){
                var workspace = EnvDteHelper.ComponentModel.GetService<VisualStudioWorkspace>();
                SolutionHelper.Solution = workspace.CurrentSolution;
                await SolutionHelper.GetCompilationsAsync(workspace.CurrentSolution);
                editor.ViewModel.SetStackTrace((string)e.NewValue);
            }
        }
    }
}
