using System.Management.Automation;
using Provider.NavigationProviderParams;

namespace PsHuddle.NavigationProviderParams
{
    public class HuddleDriveInfo : PSDriveInfo
    {
        private static  DocumentLibraryParameters _driveParams;

        public HuddleDriveInfo(PSDriveInfo driveInfo, DocumentLibraryParameters driveParams)
            : base(driveInfo)
        {
            _driveParams = driveParams;
        }

        public static string Host
        {
            get { return _driveParams.Host; }
        }
    }
}
