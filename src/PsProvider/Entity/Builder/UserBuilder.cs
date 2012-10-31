using System.Collections.Generic;
using DynamicRest.Xml;
using PsHuddle.Entity.Entities;

namespace PsHuddle.Entity.Builder
{
    public class UserBuilder
    {
        public static HuddleResourceObject Build(dynamic response)
        {
            Links links = LinkBuilder.Build(response);

            dynamic personalProfile = response.profile != null ? response.profile.personal : null;

            List<Workspace> builtWorkspaces = new List<Workspace>();
            if (response.membership != null)
            {
                XmlNodeList workspaces = response.membership.workspaces.Nodes as XmlNodeList;

                dynamic dyn_workspaces = ((dynamic)response.membership.workspaces).Nodes as XmlNodeList;
                foreach (dynamic workspace in dyn_workspaces)
                {
                    builtWorkspaces.Add(WorkSpaceBuilder.Build(workspace));
                }
            }

            return new User(links, personalProfile, builtWorkspaces);
        }
    }

    public static class DynamicExtensions
    {
        public static dynamic Get(this XmlNode element, string rel, string elementName)
        {
            var nodeList = ((dynamic)element).Nodes as XmlNodeList;
            foreach (dynamic el in nodeList)
            {
                if (el.Name == elementName && el.rel == rel) return el;
            }
            return null;
        }
    }
}