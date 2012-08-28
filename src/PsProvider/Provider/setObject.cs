using System.Management.Automation;
using Puddle.Parsing.Objects.Entities;
using Puddle.parsing.Objects;

namespace Puddle
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
            var folder = new Folder(Title, Desc);
            WriteObject(folder);

            base.ProcessRecord();
        }
    }
}
