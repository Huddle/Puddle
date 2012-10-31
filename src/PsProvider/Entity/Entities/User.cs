using System.Collections.Generic;

namespace PsHuddle.Entity.Entities
{
    public class User : HuddleResourceObject
    {
        public User(Links links, dynamic personalProfile, IEnumerable<Workspace> workspaces)
        {
            base.Links = links;
            if (personalProfile != null)
            {
                DisplayName = personalProfile.displayname;
            }
            Workspaces = workspaces;
        }

        public string DisplayName { get; set; }
        public IEnumerable<Workspace> Workspaces { get; private set; }
    }
}