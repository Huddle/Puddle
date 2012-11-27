using System;
using System.Configuration;
using System.Management.Automation;

namespace Huddle.Ps.Provider
{
    public class HuddlePSDriveInfo : PSDriveInfo
    {
        public HuddlePSDriveInfo(PSDriveInfo drive, HuddleDriveParameters parameters)
            : base(drive)
        {
            this.Host = parameters.Host;
        }

        public string Host { get; private set; }

        public void CloseAnythingWeWishToClose()
        {
            
        }

        public void OpenAnythingWeWishToOpen()
        {
            
        }
    }
}