using System.Management.Automation;

namespace Provider.NavigationProviderParams
{
    public class DocumentLibraryParameters
    {
        [Parameter(Mandatory = true)]
        public string Host { get; set; }

    }
}
