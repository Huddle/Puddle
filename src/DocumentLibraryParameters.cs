using System;
using System.Management.Automation;

namespace Puddle
{
    public class DocumentLibraryParameters
    {
        [Parameter(Mandatory = true)]
        public string Host { get;set; }
    }
}
