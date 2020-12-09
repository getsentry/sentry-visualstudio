using System;

namespace VSSentry
{
    public class GuidAndCmdID
    {
        /// <summary>
        /// VSSentryPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "b7e7885f-a848-48ef-a78b-6bf95c730b54";

        public const string PackageCmdSetGuidString = "FD3AA9D2-6CF8-46F4-879C-DF3C38C07B9C";

        public static readonly Guid guidPackage = new Guid(PackageGuidString);
        public static readonly Guid guidCmdSet = new Guid(PackageCmdSetGuidString);
    }
}