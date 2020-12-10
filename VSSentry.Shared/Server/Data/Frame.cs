using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using VSSentry.Shared.Helpers;

namespace VSSentry.Shared.Server.Data
{

    public class Frame
    {
        public Frame()
        {
            GoToMethodCommand = new DelegateCommand((_) => GoToMethod(this, null));
        }
        public string absPath { get; set; }

        public int? colNo { get; set; }
        public object[] context { get; set; }
        public object errors { get; set; }
        public string filename { get; set; }
        public string function { get; set; }
        public bool inApp { get; set; }
        public object instructionAddr { get; set; }
        public int? lineNo { get; set; }
        public string module { get; set; }
        public string package { get; set; }
        public object platform { get; set; }
        public object rawFunction { get; set; }
        public object symbol { get; set; }
        public object symbolAddr { get; set; }
        public object trust { get; set; }
        public object vars { get; set; }

        public string Text => ToString();

        public ICommand GoToMethodCommand { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("at ");
            sb.Append(module);
            sb.Append(".");
            sb.Append(function);
            sb.Append("()");
            if (lineNo != null)
            {
                sb.Append(" in ");
                sb.Append(filename);
                sb.Append(":line ");
                sb.Append(lineNo);
            }
            return sb.ToString();
        }
        public void GoToMethod(object sender, EventArgs e)
        {
            var path = Find(filename);
            if (File.Exists(path) && lineNo != null)
            {
                _ = DTEHelper.Instance.GotoFileLineAsync(path, lineNo.Value);
            }
        }

        /// <summary>
        /// Given a path to a file, try to find a project item that closely matches the file path, 
        /// but is not an exact match
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Find(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;

            path = path.Replace('/', Path.DirectorySeparatorChar);

            if (File.Exists(path)) return path;

            var pathParts = path.Split(Path.DirectorySeparatorChar);

            for (var i = 0; i < pathParts.Length; i++)
            {
                var partialPath = string.Join(Path.DirectorySeparatorChar.ToString(), pathParts.Skip(i));
                var file = DTEHelper.Instance.GetSolutionFile(partialPath);
                if (file != null)
                {
                    return file;
                }
            }

            return path;
        }

        public bool Clickable
        {
            get
            {
                return function != null && lineNo != null && inApp;
            }
        }
    }
}
