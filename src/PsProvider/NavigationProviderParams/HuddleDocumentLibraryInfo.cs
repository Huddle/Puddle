using System.Management.Automation;

namespace Provider.NavigationProviderParams
{
    public class HuddleDocumentLibraryInfo : PSDriveInfo
    {
        private static  DocumentLibraryParameters _driveParams;

        public HuddleDocumentLibraryInfo(PSDriveInfo driveInfo, DocumentLibraryParameters driveParams)
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
