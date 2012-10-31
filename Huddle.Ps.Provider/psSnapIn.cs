using System.ComponentModel;
using System.Management.Automation;

namespace Huddle.Ps.Provider
{
    [RunInstaller(true)]
    public class PsSnapIn : PSSnapIn
    {
        public override string Name
        {
            get { return "Huddle.Ps.Provider"; }
        }

        public override string Vendor
        {
            get { return "Huddle"; }
        }

        public override string Description
        {
            get { return "Provides cmdlets to help in writing psake scripts."; }
        }
    }
}