using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Text.RegularExpressions;
using DynamicRest.Fluent;

namespace Puddle
{
    [CmdletProvider("Puddle", ProviderCapabilities.None)]
    public class HuddleDocumentLibraryProvider : ContainerCmdletProvider
    {
        private readonly User _user;
        private const string ACCEPT_HEADER = "application/vnd.huddle.data+xml";

        public HuddleDocumentLibraryProvider()
        {
            _user = new User(29282750);
        }

        private static string ExtractResourceFromPath(string path)
        {
            string resource = string.Empty;
            var match = Regex.Match(path, @"(?:membership::)?(?:\w+:[\\/])?(?<folder>.*)$");

            if (match.Success)
            {
                resource = match.Groups["folder"].Value;
            }
            return resource;
        }

        private Folder GetFolderFromPath(string path)
        {
            var resource = ExtractResourceFromPath(path);

            var restClientBuilder = new RestClientBuilder()
                .WithUri(GetFolderUri(resource))
                .WithOAuth2Token(_user.Token)
                .WithAcceptHeader(ACCEPT_HEADER );

 
            var response = restClientBuilder.Build().Get();

            return new FolderFactory(response);
        }

        protected string GetFolderUri(string path)
        {
            var drive = this.PSDriveInfo as HuddleDocumentLibraryInfo;
            var apiHost = drive.Host;
            return "https://" + apiHost + "/" + path;
        }

        protected override void GetItem(string path)
        {
            var folder = GetFolderFromPath(path);
            if (folder != null)
            {
                WriteItemObject(folder, path, false);
            }
        }

        protected override bool ItemExists(string path)
        {
            return GetFolderFromPath(path) != null;
        }

        protected override bool IsValidPath(string path)
        {
            return true;
        }
        
        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            if (drive is HuddleDocumentLibraryInfo)
            {
                return drive;
            }
            
            var libraryParams = this.DynamicParameters as DocumentLibraryParameters;
            return new HuddleDocumentLibraryInfo(driveInfo: drive, driveParams: libraryParams);
        }

        protected override object NewDriveDynamicParameters()
        {
            return new DocumentLibraryParameters();
        }

    }
}
