using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSSentry.Shared.Server.Data
{

    public class Frame
    {
        public string absPath { get; set; }
        public bool Clickable
        {
            get
            {
                return function != null && lineNo != null && inApp;
            }
        }

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

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(module);
            sb.Append(" at ");
            sb.Append(function);
            if(lineNo != null)
            {
                sb.Append(":");
                sb.Append(lineNo);
            }
            return sb.ToString();
        }
        public void GoToMethod(object sender, EventArgs e)
        {

        }
    }
}
