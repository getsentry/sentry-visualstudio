using System;
using System.Threading.Tasks;
using EnvDTE;
using VSSentry.Shared.Helpers;

namespace VSSentry.Helpers
{
    /// <summary>
    /// Instance based DTE helper to be called from shared libraries
    /// </summary>
    public class DteHelper : IDTEHelper
    {
        private readonly DTE _dte;

        public DteHelper(DTE dte)
        {
            _dte = dte;
        }

        public string GetSolutionFile(string path)
        {
            var projectItem = _dte.Solution.FindProjectItem(path);
            if (projectItem != null)
            {
                return projectItem.FileNames[0];
            }
            return null;
        }

        public async Task<bool> GotoFileLineAsync(string path, int lineNumber)
        {
            await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            _dte.ExecuteCommand("File.OpenFile", path);

            try
            {
                (_dte.ActiveDocument?.Selection as TextSelection)?.GotoLine(lineNumber);
            }
            catch (Exception)
            {
                // Cannot go to the requested line in the file
                return false;
            }
            return true;
        }
    }
}
