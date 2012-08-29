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

/*
 * HUGE NOTICE
 * Because of how dynamic rest works try x if x doesn't work it means do y 
 * it means you'll get a lot of exceptions when debugging
 * therefore I have disabled thrown messages with xmlException and invalidOperation
 * TURN THESE BACK ON IF YOU NEED THEM but be prepared to mash f5 10 times or so 
 */
namespace PSHuddle
{
    [CmdletProvider("PsHuddle", ProviderCapabilities.ShouldProcess | ProviderCapabilities.Filter)]
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
            var pathItem = _responseItemFactory.Create(getPath.Get()); //we do this to get the folders create-links

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

        protected override object NewDriveDynamicParameters()
        {
            return new DocumentLibraryParameters();
        }

        protected override string MakePath(string parent, string child)
        {

            //we need to check if the match path is a real path or if its name of the folder
            const string pattern = @"/(\d+)$";
            var match = Regex.Match(child, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return child;
            }

            return child.EndsWith("*") ? GetChildMatcheFromParent(child) : GetChildNameFromParentName(child);
        }

        private string GetChildMatcheFromParent(string match)
        {
            return string.Empty;
            match = match.Substring(0, match.Length - 1);
            var resource = new ResourceFinder(PSDriveInfo.CurrentLocation, _token, AcceptHeader);
            var item = _responseChildItemFactory.Create(resource.Get());

            var selectedItem=  item.FirstOrDefault(l => l.Title.StartsWith(match,StringComparison.CurrentCultureIgnoreCase));
            return selectedItem.Title;
        }

        private string GetChildNameFromParentName(string parent)
        {
            if(parent == "" || parent == "entry")
            {
                return parent;
            }

            var parentDivider = parent.LastIndexOf('/');

            var resource = new ResourceFinder(PSDriveInfo.CurrentLocation, _token, AcceptHeader);
            
            //check to see if the word they typed is an item
            var item = _responseChildItemFactory.Create(resource.Get());
            foreach (var childItem in item) //now we have the child items find the 1 with our name 
            {
                if (childItem == null)
                    continue;

                if (parentDivider > 0 && 
                    childItem.Title.Equals(parent.Substring(parentDivider + 1),StringComparison.CurrentCultureIgnoreCase)) //found the bugger now return its self link
                {
                    var selfLink = childItem.Links.Single(l => l.Rel == "self").Href;
                    return selfLink;
                }
                if (childItem.Title == parent)
                {
                    var selfLink = childItem.Links.Single(l => l.Rel == "self").Href;
                    return selfLink;
                }
            }

            var linksInItem = _responseItemFactory.Create(resource.Get()).Links;
            //still not found? check if its a link they are looking for
            Link link;
            if (parentDivider > 0)
            {
                link = linksInItem.FirstOrDefault(l => l.Rel == parent.Substring(parentDivider + 1));
                if (link != null)
                    return link.Href;
            }

            link = linksInItem.FirstOrDefault(l => l.Rel == parent.Substring(parentDivider + 1));
            if (link != null)
            {
                return link.Href;
            }

            return parent;

        }

        protected override string GetChildName(string path)
        {
            const string pattern = @"[\d]+";
            Match match = Regex.Match(path, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return match.Groups[0].Value;
        }

        protected override string GetParentPath(string path, string root)
        {
            const string pattern = @".+[^/]+/";

            Match match = Regex.Match(path, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return root + match.Groups[1].Value;
            }

            return root + match.Groups[0].Value;
        }
    }
}


