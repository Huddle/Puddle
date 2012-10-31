using System.Management.Automation;
using Provider.Resource;
using PsHuddle.Entity;
using Token.OAuth2;

namespace Provider
{
    [Cmdlet("undo", "delete")]
    public class UndoDelete : PSCmdlet
    {
        private string _desc;
        private GetToken _token = new GetToken();
        private const string AcceptHeader = "application/vnd.huddle.data+xml";
        private readonly ResponseItemFactory _responseItemFactory = new ResponseItemFactory();

        #region Parameters

        /// <summary>
        /// The names of the processes retrieved by the cmdlet.
        /// </summary>


        /// <summary>
        /// Gets or sets the list of process names on which 
        /// the Get-Proc cmdlet will work.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Path { get; set; }

        #endregion Parameters

        protected override void ProcessRecord()
        {
            var resource = new ResourceUndoDelete(Path + "/restore", _token, AcceptHeader);
            var item = _responseItemFactory.Create(resource.Put());
            WriteObject(item);
            base.ProcessRecord();
        }
    }
}
