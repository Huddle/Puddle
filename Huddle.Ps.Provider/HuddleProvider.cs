using System;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace Huddle.Ps.Provider
{
    [CmdletProvider("Huddle.Ps.Provider", ProviderCapabilities.ShouldProcess | ProviderCapabilities.Filter)]
    public class HuddleProvider : ItemCmdletProvider
    {
        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            //if (drive is HuddlePSDriveInfo) return drive;
            //if (drive == null)
            //{
            //    WriteError(new ErrorRecord(new ArgumentNullException("drive"), "NullDrive", ErrorCategory.InvalidArgument, null));
            //    return null;
            //}
            ////// check if drive root is not null or empty and if its an existing file
            ////if (String.IsNullOrEmpty(drive.Root) || (File.Exists(drive.Root) == false))
            ////{
            ////    WriteError(new ErrorRecord(new ArgumentException("drive.Root"), "NoRoot", ErrorCategory.InvalidArgument, drive) );
            ////    return null;
            ////}

            // create a new drive and use dynamic parameters to pass through
            var huddlePSDriveInfo = new HuddlePSDriveInfo(drive, DynamicParameters as HuddleDriveParameters);
            //drive.
            return huddlePSDriveInfo;
        }
        protected override object NewDriveDynamicParameters()
        {
            return new HuddleDriveParameters();
        }
        protected override PSDriveInfo RemoveDrive(PSDriveInfo drive)
        {
            if (drive == null)
            {
                WriteError(new ErrorRecord(new ArgumentNullException("drive"), "NullDrive", ErrorCategory.InvalidArgument, drive));
                return null;
            }
            var huddlePsDriveInfo = drive as HuddlePSDriveInfo;
            if (huddlePsDriveInfo != null)
            {   
                huddlePsDriveInfo.CloseAnythingWeWishToClose();
            }   
            return huddlePsDriveInfo;
        }

        protected override bool IsValidPath(string path)
        {
            if (String.IsNullOrWhiteSpace(path)) return false;

            return NormalizePath(path).Split("\\".ToCharArray()).Length != 0;
        }
        
        private string NormalizePath(string path)
        {
            return path.Replace("/", "\\");
        }
    }

    public class HuddleDriveParameters
    {
        [Parameter(Mandatory = true)]
        public string Host { get; set; }
    }
}
