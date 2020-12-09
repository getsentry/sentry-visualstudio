using Microsoft.VisualStudio.Language.CodeLens;
using Microsoft.VisualStudio.Language.CodeLens.Remoting;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VSSentry.CodeLens
{
    [Export(typeof(IAsyncCodeLensDataPointProvider))]
    [Name(ProviderId)]
    [ContentType("CSharp")]
    [LocalizedName(typeof(Resources), "Name")]
    //[Priority(201)] // Sort after "references" CodeLens (200)
    public class SentryCodeLensProvider : IAsyncCodeLensDataPointProvider
    {
        public const string ProviderId = "SentryErrors";


        public async Task<bool> CanCreateDataPointAsync(CodeLensDescriptor descriptor, CodeLensDescriptorContext descriptorContext, CancellationToken token)
        {
            if (descriptor.Kind == CodeElementKinds.Method)
            {
                var projectId = descriptor.ProjectGuid;
                var connection = Shared.Server.SentryConnection.GetCurrent(projectId);
                return connection.IsEnabled;
            }
            return false;
        }


        public async Task<IAsyncCodeLensDataPoint> CreateDataPointAsync(CodeLensDescriptor descriptor, CodeLensDescriptorContext descriptorContext, CancellationToken token)
        {
            var projectId = descriptor.ProjectGuid;
            var connection = Shared.Server.SentryConnection.GetCurrent(projectId);
            //string pathBase;
            //if (!_pathBases.TryGetValue(descriptor.ProjectGuid, out pathBase))
            //{
            //    pathBase = FindPathBase(descriptor.FilePath);
            //    _pathBases[descriptor.ProjectGuid] = pathBase;
            //}

            return new SentryDataPoint(connection, descriptor);
        }


        /// <summary>
        /// We strip any user-specific path base by looking for the .sln file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string FindPathBase(string path)
        {
            var directory = Directory.GetParent(path);
            while (!directory.GetFiles().Any(x => x.Extension == "sln"))
            {
                directory = Directory.GetParent(directory.FullName);
                if (directory == null)
                {
                    break;
                }
            }

            // We couldn't find an SLN file, look for CSPROJ
            if (directory == null)
            {
                directory = Directory.GetParent(path);
                while (!directory.GetFiles().Any(x => x.Extension == "csproj"))
                {
                    directory = Directory.GetParent(directory.FullName);
                    if (directory == null)
                    {
                        break;
                    }
                }
            }

            return directory == null ? string.Empty : Directory.GetParent(directory.FullName).FullName;
        }
    }
}
