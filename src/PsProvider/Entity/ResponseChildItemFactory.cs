using System.Collections.Generic;
using System.Xml;
using PsHuddle.Entity.Builder;
using PsHuddle.Entity.Entities;

namespace PsHuddle.Entity
{
    public class ResponseChildItemFactory
    {
        public List<HuddleResourceObject> Create(dynamic response)
        {
            List<HuddleResourceObject> childItem = new List<HuddleResourceObject>();


            //horrible way to do this however dynamic rest has no way to check if a node exists
            //so according to Francesco at least the easiest way to do is is to try the parsing if it fails
            //it means the node is not there
            childItem.AddRange(CreateWorkSpaces(response));
            childItem.AddRange(CreateDocuments(response));
            childItem.AddRange(CreateFolders(response));

            return childItem;
        }


        private IEnumerable<HuddleResourceObject> CreateWorkSpaces(dynamic response) 
        {
            var workspaces = new List<HuddleResourceObject>();

            try
            {
                dynamic workspaceResponse = response.Result.membership.workspaces;
                if (workspaceResponse.Count == 1)
                {
                    workspaces.Add(WorkSpaceBuilder.Build(workspaceResponse.workspace));
                }
                else
                {
                    foreach (dynamic ws in workspaceResponse.workspace)
                    {
                        workspaces.Add(WorkSpaceBuilder.Build(ws));
                    }                
                }
            }
            catch (XmlException){}

            return workspaces;
        }

        private IEnumerable<HuddleResourceObject> CreateDocuments(dynamic response)
        {
            var documents = new List<HuddleResourceObject>();

            try
            {
                if (response.Result.documents.Count == 1)
                {
                    documents.Add(DocumentBuilder.Build(response.Result.documents.document));
                }
                else
                {
                    foreach (dynamic dc in response.Result.documents.document)
                    {
                        documents.Add(DocumentBuilder.Build(dc));
                    }
                }
            }
            catch (XmlException) { }

            return documents;
        }

        private IEnumerable<HuddleResourceObject> CreateFolders(dynamic response)
        {
            var folders = new List<HuddleResourceObject>();

            try
            {
                if (response.Result.folders.Count == 1)
                {
                    folders.Add(FolderBuilder.Build(response.Result.folders.folder));
                }
                else
                {
                    foreach (dynamic fd in response.Result.folders.folder)
                    {
                        folders.Add(FolderBuilder.Build(fd));
                    }
                }
               // return folders;
            }
            catch (XmlException) { }

            return folders;
        }
    }



}
