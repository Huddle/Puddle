using System.Management.Automation;
using Provider.Entity.Entities;

namespace Provider
{
    [Cmdlet("New", "fileLocation")]
    public class NewFileLocation : PSCmdlet
    {

        #region Parameters

        /// <summary>
        /// The names of the processes retrieved by the cmdlet.
        /// </summary>


        /// <summary>
        /// Gets or sets the list of process names on which 
        /// the Get-Proc cmdlet will work.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Path{ get; set; }



        #endregion Parameters

        protected override void ProcessRecord()
        {
            WriteObject(Path);

            base.ProcessRecord();
        }
    }
}
