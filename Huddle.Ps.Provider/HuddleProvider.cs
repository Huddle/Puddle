using System;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace Huddle.Ps.Provider
{
    [CmdletProvider("Huddle.Ps.Provider", ProviderCapabilities.ShouldProcess | ProviderCapabilities.Filter)]
    public class HuddleProvider : DriveCmdletProvider
    {
        public HuddleProvider()
        {
            Console.Write("gere");
        }

        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            if (drive is HuddlePSDriveInfo) return drive;
            if (drive == null)
            {
                WriteError(new ErrorRecord(new ArgumentNullException("drive"), "NullDrive", ErrorCategory.InvalidArgument, null));
                return null;
            }
            //// check if drive root is not null or empty
            //// and if its an existing file
            //if (String.IsNullOrEmpty(drive.Root) || (File.Exists(drive.Root) == false))
            //{
            //    WriteError(new ErrorRecord(
            //        new ArgumentException("drive.Root"),
            //        "NoRoot",
            //        ErrorCategory.InvalidArgument,
            //        drive)
            //    );

            //    return null;
            //}

            // create a new drive and create an ODBC connection to the new drive
            var huddlePSDriveInfo = new HuddlePSDriveInfo(drive);
            return huddlePSDriveInfo;
        }

        protected override PSDriveInfo RemoveDrive(PSDriveInfo drive)
        {
            if (drive == null)
            {
                WriteError(new ErrorRecord(new ArgumentNullException("drive"), "NullDrive", ErrorCategory.InvalidArgument, drive));
                return null;
            }
            var huddlePsDriveInfo = drive as HuddlePSDriveInfo;
            if (huddlePsDriveInfo == null)
            {
                return null;
            }

            huddlePsDriveInfo.CloseAnythingWeWishToClose();
            return huddlePsDriveInfo;
        }
    }
}
