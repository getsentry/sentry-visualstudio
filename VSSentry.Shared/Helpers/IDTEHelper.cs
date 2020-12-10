using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSSentry.Shared.Helpers
{
    public static class DTEHelper
    {
        public static IDTEHelper Instance { get; set; }
    }

    public interface IDTEHelper
    {
        string GetSolutionFile(string path);
        Task<bool> GotoFileLineAsync(string path, int lineNumber);
    }
}
