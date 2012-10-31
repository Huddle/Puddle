using System.Management.Automation;
using PsHuddle.Entity.Entities;

namespace Provider
{
    [Cmdlet("New", "HuddleObject")]
    public class NewHuddleObject : PSCmdlet
    {
        private string _title;
        private string _desc;

        /// <summary>
        /// Gets or sets the list of process names on which 
        /// the Get-Proc cmdlet will work.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }

        [Parameter(Mandatory = true)]
        public string Desc
        {
            get { return this._desc; }
            set { this._desc = value; }
        }

        protected override void ProcessRecord()
        {
            var item = new HuddleResourceObject {Title = Title, Description = Desc};

            WriteObject(item);

            base.ProcessRecord();
        }
    }
}
