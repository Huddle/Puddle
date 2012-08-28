using System.Management.Automation;
using Provider.Entity.Entities;

namespace Provider
{
    [Cmdlet("New", "object")]
    public class SetObject : PSCmdlet
    {
        private string _title;
        private string _desc;

        #region Parameters

        /// <summary>
        /// The names of the processes retrieved by the cmdlet.
        /// </summary>


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

        #endregion Parameters

        protected override void ProcessRecord()
        {
            var item = new HuddleResourceObject();
            item.Title = Title;
            item.Description = Desc;

            WriteObject(item);

            base.ProcessRecord();
        }
    }
}
