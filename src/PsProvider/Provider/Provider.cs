using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Text.RegularExpressions;
using Provider.Entity;
using Provider.Entity.Entities;
using Provider.NavigationProviderParams;
using Provider.Resource;
using Token.OAuth2;

namespace Provider
{
    [CmdletProvider("Provider", ProviderCapabilities.ShouldProcess | ProviderCapabilities.Filter)]
    public class Provider : NavigationCmdletProvider
    {
        private const string AcceptHeader = "application/vnd.huddle.data+xml";
        private readonly GetToken _token = new GetToken();
        private readonly ResponseItemFactory _responseItemFactory = new ResponseItemFactory();
        private readonly ResponseChildItemFactory _responseChildItemFactory = new ResponseChildItemFactory();

        protected override void GetItem(string path)
        {
            var resource = new ResourceFinder(path, _token, AcceptHeader);
            var item = _responseItemFactory.Create(resource.Get());

            var selfLink = item.Links.Single(l => l.Rel == "self").Href;
            WriteItemObject(item, selfLink, IsItemContainer(selfLink));
        }

        protected override void RemoveItem(string path, bool recurse)
        {
            var resource = new ResourceRemover(path, _token, AcceptHeader);
            resource.Delete();
        }

        protected override void GetChildItems(string path, bool recurse)
        {
            var resource = new ResourceFinder(path, _token, AcceptHeader);
            var item = _responseChildItemFactory.Create(resource.Get());

            foreach (var childItem in item)
            {
                if (childItem == null)
                    continue;

                var selfLink = childItem.Links.Single(l => l.Rel == "self").Href;
                WriteItemObject(childItem, selfLink, IsItemContainer(selfLink));
            }
        }

        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {

            PSObject fileEntity = PSObject.AsPSObject(newItemValue);
            var itemEntityValue = fileEntity.Properties.ToArray();
            var xmlBody = "";

            var getPath = new ResourceFinder(path, _token, AcceptHeader);
                //we want to get the uri of the parent folder they specififed
            var pathItem = _responseItemFactory.Create(getPath.Get());
                //we do this to get the folders create-document and create-folder links


            if (itemTypeName == "folder")
            {
                path = pathItem.Links.Single(l => l.Rel == "create-folder").Href;
                xmlBody = String.Format("<folder title='{0}' description='{1}' />", itemEntityValue[2].Value,
                                        itemEntityValue[1].Value);
            }
            else if (itemTypeName == "document")
            {
                path = pathItem.Links.Single(l => l.Rel == "create-document").Href;
                xmlBody = String.Format("<document title='{0}' description='{1}' />", itemEntityValue[2].Value,
                                        itemEntityValue[1].Value);
            }
            else
            {
                var error = new ArgumentException("Invalid type!");
                WriteError(new ErrorRecord(error, "Invalid Item", ErrorCategory.InvalidArgument, itemTypeName));
            }

            var resource = new ResourceCreater(path, _token, AcceptHeader, xmlBody);
            var item = _responseItemFactory.Create(resource.Post());

            var selfLink = item.Links.Single(l => l.Rel == "self").Href;
            WriteItemObject(item, selfLink, IsItemContainer(selfLink));
        }

        protected override void SetItem(string path, object value)
        {
            PSObject fileEntity = PSObject.AsPSObject(value);
            var itemEntityValue = fileEntity.Properties.ToArray();

            var resource = new ResourceFinder(path, _token, AcceptHeader);
            HuddleResourceObject item;


            if (itemEntityValue[0].Name == "uploadPath")
            {
                item = _responseItemFactory.Create(resource.Get());
                path = item.Links.Single(l => l.Rel == "upload").Href;

                var uploadResource = new ResourceUploader(path, _token, AcceptHeader,
                                                          itemEntityValue[0].Value.ToString());
                item = _responseItemFactory.Create(uploadResource.SendMutiPartRequest());
            }
            else
            {
                item = _responseItemFactory.Create(resource.Get());

                item.Title = itemEntityValue[2].Value.ToString();
                item.Description = itemEntityValue[1].Value.ToString();

                path = item.Links.Single(l => l.Rel == "edit").Href;
                var editedResource = new ResourceModifier(path, _token, AcceptHeader, item);
                editedResource.Put();
            }
        }

        protected override bool ItemExists(string path)
        {
            var resource = new ResourceFinder(path, _token, AcceptHeader);

            if (resource.Get() != null)
            {
                return true;
            }

            return true;
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

            var libraryParams = DynamicParameters as DocumentLibraryParameters;
            return new HuddleDocumentLibraryInfo(driveInfo: drive, driveParams: libraryParams);
        }

        //try removing me if stuff breaks because -> 17/08/2012 not tested
        protected override bool HasChildItems(string path)
        {
            var resource = new ResourceFinder(path, _token, AcceptHeader);
            var item = _responseChildItemFactory.Create(resource.Get());

            return item.Any();
        }

        protected override bool IsItemContainer(string path)
        {
            var resource = new ResourceFinder(path, _token, AcceptHeader);
            return resource.Get().Result.Name != "document";
        }

        protected override string GetChildName(string path)
        {
            string pattern = @"/(\d+)$";
            Match match = Regex.Match(path, pattern, RegexOptions.IgnoreCase);

            if (match.Success) //if the last bit of the uri is digits only then its a normal link so lets go to it
            {
                pattern = @"[\d]+";
                match = Regex.Match(path, pattern, RegexOptions.IgnoreCase);

                return match.Success ? match.Groups[1].Value : match.Groups[0].Value;
            }
            return GetChildNameWhenKnownParent(path);
        }

        protected override string MakePath(string parent, string child)
        {
            return "" + child;
        }

        protected override object NewDriveDynamicParameters()
        {
            return new DocumentLibraryParameters();
        }

        private string GetChildNameWhenKnownParent(string path)
        {
            var parentDivider = path.LastIndexOf('/');

            var parentPath = path.Substring(0, parentDivider);

            var resource = new ResourceFinder(parentPath, _token, AcceptHeader);
            var item = _responseChildItemFactory.Create(resource.Get());

            foreach (var childItem in item) //now we have the child items find the 1 with our name 
            {
                if (childItem == null)
                    continue;

                if (childItem.Title == path.Substring(parentDivider + 1)) //found the bugger now return its self link
                {
                    var selfLink = childItem.Links.Single(l => l.Rel == "self").Href;
                    GetChildName(selfLink);
                    break;
                }
            }

            return String.Empty;
        }
    }
}


