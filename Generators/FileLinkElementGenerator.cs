﻿// From https://github.com/sboulema/StackTraceExplorer
using System.Text.RegularExpressions;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Rendering;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using ICSharpCode.AvalonEdit;
using System.IO;
using VSSentry.Helpers;
using VSSentry.UI;

namespace VSSentry.Generators
{
    public class FileLinkElementGenerator : VisualLineElementGenerator
    {
        // To use this class:
        // textEditor.TextArea.TextView.ElementGenerators.Add(new FileLinkElementGenerator());

        private TextEditor _textEditor;
        private static readonly Regex FilePathRegex = new Regex(@"((?:[A-Za-z]\:|\\|)(?:\\[a-zA-Z_\-\s0-9\.\(\)]+)+):(?:line|Zeile)? (\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public FileLinkElementGenerator(TextEditor textEditor)
        {
            _textEditor = textEditor;
        }

        private Match FindMatch(int startOffset)
        {
            // fetch the end offset of the VisualLine being generated
            var endOffset = CurrentContext.VisualLine.LastDocumentLine.EndOffset;
            var document = CurrentContext.Document;
            var relevantText = document.GetText(startOffset, endOffset - startOffset);
            return FilePathRegex.Match(relevantText);
        }

        /// Gets the first offset >= startOffset where the generator wants to construct
        /// an element.
        /// Return -1 to signal no interest.
        public override int GetFirstInterestedOffset(int startOffset)
        {
            var m = FindMatch(startOffset);
            return m.Success ? startOffset + m.Index : -1;
        }

        /// Constructs an element at the specified offset.
        /// May return null if no element should be constructed.
        public override VisualLineElement ConstructElement(int offset)
        {
            var m = FindMatch(offset);
            // check whether there's a match exactly at offset
            if (!m.Success || m.Index != 0) return null;
            if (!File.Exists(ClickHelper.Find(m.Groups[1].Value))) return null;
            var line = new CustomLinkVisualLineText(
                new [] { m.Groups[1].Value, m.Groups[2].Value }, 
                CurrentContext.VisualLine, 
                m.Groups[0].Length, 
                ToBrush(EnvironmentColors.ControlLinkTextColorKey),
                ClickHelper.HandleFileLinkClicked,
                false,
                CurrentContext.Document,
                _textEditor,
                null
            );

            if (EnvDteHelper.ViewModel.IsClickedLine(line))
            {
                line.ForegroundBrush = ToBrush(EnvironmentColors.StatusBarNoSolutionColorKey);
            }

            return line;
        }

        private static SolidColorBrush ToBrush(ThemeResourceKey key)
        {
            var color = VSColorTheme.GetThemedColor(key);
            return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }
    }
}
