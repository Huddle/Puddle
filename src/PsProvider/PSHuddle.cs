using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Text.RegularExpressions;
using DynamicRest;
using Provider.NavigationProviderParams;
using Provider.Resource;
using PsHuddle.Entity;
using PsHuddle.Entity.Entities;
using PsHuddle.NavigationProviderParams;
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
    public class PSHuddle : NavigationCmdletProvider
    {
        private string pathSeparator = "\\";
        private const string AcceptHeader = "application/vnd.huddle.data+xml";
        private readonly GetToken _token = new GetToken();
        private readonly ResponseItemFactory _responseItemFactory = new ResponseItemFactory();
        private readonly ResponseChildItemFactory _responseChildItemFactory = new ResponseChildItemFactory();
        private static List<HuddleResourceObject> _currentChildItems;

        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            if (drive is HuddleDriveInfo)
            {
                return drive;
            }

            var libraryParams = DynamicParameters as DocumentLibraryParameters;
            return new HuddleDriveInfo(drive, libraryParams);
        }

        public static HuddleResourceObject CurrentItem { get; set ; }
        public static User CurrentUser { get; set; }

        private static readonly Regex _matchPath = GetRegEx(@"/(\d+)$");
        private static readonly Regex matchChildName = GetRegEx(@"[\d]+");
        private static readonly Regex _matchParentPath = GetRegEx(@".+[^/]+/");
        private static List<HuddleResourceObject> CurrentChildItems
        {
            get { return _currentChildItems ?? new List<HuddleResourceObject>(); }
            set { _currentChildItems = value; }
        }
        private static Regex GetRegEx(string pattern)
        {
            return new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        protected override void GetItem(string path)
        {
            WriteDebug("GetItem - Path:" + path);
            
            if (IgnoreRequestFromPowershell(path) )
            {
                if (CurrentUser == null)
                {
                    CurrentItem = GetEntityFromPath(path);
                    CurrentUser = (User)CurrentItem;
                }
                WriteItemObject(PSDriveInfo, path, true);
                return;
            }

            CurrentItem = GetEntityFromPath(path);
            //if (_currentItem is Workspace)
            //{
            //    WriteWarning("Workspace '' detected, selecting documentLibrary" + _currentItem.Title);

            //    //select Doc Library
                
            //    //_currentItem = GetEntityFromPath(((Workspace)_currentItem).LinkDocumentLibrary);

            //    GetItem(((Workspace)_currentItem).LinkDocumentLibrary);
            //    return;
            //}

            WriteItemObject(CurrentItem, CurrentItem.LinkSelf, IsItemContainer(CurrentItem.LinkSelf));
        }

        protected override void GetChildItems(string path, bool recurse)
        {
            WriteDebug("GetCI - Path:" + path);

            CurrentChildItems = GetEntitiesFromPath(path);
            foreach (var childItem in CurrentChildItems)
            {
                if (childItem == null)
                    continue;

                WriteItemObject(childItem, childItem.LinkSelf, IsItemContainer(childItem.LinkSelf));
            }
            if (!CurrentChildItems.Any())
            {
                WriteWarning("Empty container " + CurrentItem.Title);
            }
        }

        protected override void RemoveItem(string path, bool recurse)
        {
            var resource = new ResourceRemover(path, _token, AcceptHeader);
            resource.Delete();
        }

        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {
            WriteDebug("NewItem - Path:" + path + " Type:" + itemTypeName);

            PSObject fileEntity = PSObject.AsPSObject(newItemValue);
            var itemEntityValue = fileEntity.Properties.ToArray();
            var xmlBody = "";

            var pathItem = GetEntityFromPath(path);
            if (itemTypeName == "folder")
            {
                path = pathItem.LinkFor("create-folder");
                xmlBody = String.Format("<folder title='{0}' description='{1}' />", itemEntityValue[2].Value,
                                        itemEntityValue[1].Value);
            }
            else if (itemTypeName == "document")
            {
                path = pathItem.LinkFor("create-document");
                xmlBody = String.Format("<document title='{0}' description='{1}' />", itemEntityValue[2].Value,
                                        itemEntityValue[1].Value);
            }
            else
            {
                var error = new ArgumentException("Invalid type!");
                WriteError(new ErrorRecord(error, "Invalid Item", ErrorCategory.InvalidArgument, itemTypeName));
            }

            var item = GetEntityFromPath(path);
            WriteItemObject(item, item.LinkSelf, IsItemContainer(item.LinkSelf));
        }

        protected override void SetItem(string path, object value)
        {
            WriteDebug("SetItem - Path:" + path);

            PSObject fileEntity = PSObject.AsPSObject(value);
            var itemEntityValue = fileEntity.Properties.ToArray();

            var resource = new ResourceFinder(path, _token, AcceptHeader);
            CurrentItem = _responseItemFactory.Create(resource.Get());

            
            if (itemEntityValue[0].Name == "uploadPath")
            {
                path = CurrentItem.LinkFor("upload");
                var uploadResource = new ResourceUploader(path, _token, AcceptHeader,
                                                          itemEntityValue[0].Value.ToString());
                CurrentItem = _responseItemFactory.Create(uploadResource.SendMutiPartRequest());
            }
            else
            {
                CurrentItem.Title = itemEntityValue[2].Value.ToString();
                CurrentItem.Description = itemEntityValue[1].Value.ToString();

                path = CurrentItem.LinkFor("edit");
                var editedResource = new ResourceModifier(path, _token, AcceptHeader, CurrentItem);
                editedResource.Put();
            }
        }

        protected override bool ItemExists(string path)
        {
            if (IgnoreRequestFromPowershell(path)) return true;
            if (CurrentUser.LinkSelf == path) return true;
            if (CurrentUser.Workspaces.Any(w => w.LinkSelf == path)) return true;

            WriteDebug("ItemExists - Path:" + path);

            var resource = new ResourceFinder(path, _token, AcceptHeader);

            RestOperation restOperation = resource.Get();
            if (restOperation != null)
            {
                return true;
            }

            return true;
        }

        //try removing me if stuff breaks because -> 17/08/2012 not tested
        protected override bool HasChildItems(string path)
        {
            WriteDebug("HasCI - Path:" + path);

            return CurrentItem.IsContainer;
        }

        protected override bool IsItemContainer(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return true;
            if (CurrentUser != null && CurrentUser.LinkSelf == path) return true;
            if (CurrentUser != null && CurrentUser.Workspaces.Any(w => w.LinkSelf == path)) return true;
            if (CurrentItem != null && CurrentItem.LinkSelf == path) return !(CurrentItem is Document);

            WriteDebug("IsItemContainer - Path:" + path);

            CurrentItem = GetEntityFromPath(path);
            return CurrentItem.IsContainer;
        }

        protected override bool IsValidPath(string path)
        {
            WriteDebug("IsValidPath - Path:" + path);

            return true;
        }

        protected override object NewDriveDynamicParameters()
        {
            return new DocumentLibraryParameters();
        }

        protected override string MakePath(string parent, string child)
        {
            if (IgnoreRequestFromPowershell(child)) return string.Empty;

            WriteDebug("MakePath:" + parent + " child:" + child);

            //we need to check if the match path is a real path or if its name of the folder
            var match = _matchPath.Match(child);
            if (match.Success)
            {
                return child;
            }

            return child.EndsWith("*") ? 
                GetChildMatcheFromParent(child)
                : GetChildNameFromParentName(child);
        }

        private string GetChildMatcheFromParent(string match)
        {
            if (IgnoreRequestToProvider(match)) return string.Empty;

            WriteDebug("GetChildMatchedFrom - Path:" + match);

            HuddleResourceObject matchedItem = CurrentChildItems.FirstOrDefault(ci => ci.Title.StartsWith(match.TrimEnd('*')));
            if (matchedItem == null)
            {
                return string.Empty;
            }
            return matchedItem.Title;
        }

        private string GetChildNameFromParentName(string parent)
        {
            if (IgnoreRequestToProvider(parent)) return string.Empty;

            HuddleResourceObject matchedItem = CurrentChildItems.FirstOrDefault(ci => ci.Title == parent);
            if (matchedItem != null) return matchedItem.LinkSelf;

            WriteDebug("GetChildNameFromParentName:" + parent);

            if (parent == "" || parent == "entry")
            {
                return parent;
            }

            var parentDivider = parent.LastIndexOf('/');
            var resource = new ResourceFinder(PSDriveInfo.CurrentLocation, _token, AcceptHeader);
            
            //check to see if the word they typed is an item
            RestOperation restOperation = resource.Get();
            CurrentChildItems = _responseChildItemFactory.Create(restOperation);
            foreach (var childItem in CurrentChildItems) //now we have the child items find the 1 with our name 
            {
                if (childItem == null)
                    continue;

                if (parentDivider > 0 && 
                    childItem.Title.Equals(parent.Substring(parentDivider + 1),StringComparison.CurrentCultureIgnoreCase)) //found the bugger now return its self link
                {
                    return childItem.LinkSelf;
                }
                if (childItem.Title == parent)
                {
                    return childItem.LinkSelf;
                }
            }

            var linksInItem = _responseItemFactory.Create(restOperation).Links;

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
        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            foreach (var childItem in CurrentChildItems.Where(ci => ci.LinkSelf == path))
            {
                WriteItemObject(childItem.Title, path, childItem.IsContainer);
            }
        }

        protected override string GetChildName(string path)
        {
            WriteDebug("GetChildName:" + path);

            if (CurrentChildItems.Any(ci => ci.LinkSelf == path))
                return CurrentChildItems.Single(ci => ci.LinkSelf == path).Title;

            Match match = matchChildName.Match(path);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return match.Groups[0].Value;
        }

        protected override string GetParentPath(string path, string root)
        {
            if (CurrentUser != null && path == CurrentUser.LinkSelf) return CurrentUser.LinkSelf;
            if (CurrentChildItems.Any(ci => ci.LinkSelf == path))
                return CurrentItem.LinkSelf;

            WriteDebug("GetParentParh:" + path + " root:" + root);

            Match match = _matchParentPath.Match(path);
            if (match.Success)
            {
                return root + match.Groups[1].Value;
            }

            return root + match.Groups[0].Value;
        }

        private bool IgnoreRequestToProvider(string path)
        {
            return path.Trim() == ".git";
        }
        private bool IgnoreRequestFromPowershell(string path)
        {
            return PathIsDrive(path) || path.Trim() == ".git";
        }

        private static void WriteDebug(string message)
        {
            Debug.WriteLine(message);
        }

        private HuddleResourceObject GetEntityFromPath(string path)
        {
            var resource = new ResourceFinder(path, _token, AcceptHeader);
            RestOperation restOperation = resource.Get();
            return _responseItemFactory.Create(restOperation);
        }
        private List<HuddleResourceObject> GetEntitiesFromPath(string path)
        {
            var resource = new ResourceFinder(path, _token, AcceptHeader);
            RestOperation restOperation = resource.Get();
            return _responseChildItemFactory.Create(restOperation);
        }

        private bool PathIsDrive(string path)
        {
            return String.IsNullOrEmpty(path);
        }
    }
}


