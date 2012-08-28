using System.Management.Automation;

namespace Puddle
{
    public class HuddleDocumentLibraryInfo : PSDriveInfo
    {
        private readonly DocumentLibraryParameters _driveParams;

        public HuddleDocumentLibraryInfo(PSDriveInfo driveInfo, DocumentLibraryParameters driveParams) 
            : base(driveInfo)
        {
            _driveParams = driveParams;
        }

        public string Host
        {
            get { return _driveParams.Host; }
        }
    }
}
