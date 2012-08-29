using System.Management.Automation;
using System.ComponentModel;

namespace Puddle
{
    [RunInstaller(true)]
    public class SnapIn : PSSnapIn
    {
        public override string Description
        {
            get { return "Custom Provider for Huddle"; }
        }

        public override string Name
        {
            get { return "PsHuddle"; }
        }

        public override string Vendor
        {
            get { return "Huddle"; }
        }
    }
}
