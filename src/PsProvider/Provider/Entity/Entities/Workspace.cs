using System.IO;

namespace Provider.Entity.Entities
{
    public class Workspace : HuddleResourceObject
    {
        public Workspace(string type, string title, Links links)
        {
            Links = links;
            Title = title;
            Type = type;
        }

        public Workspace()
        {
            
        }

        public string Type { get; private set; }
        public FileAttributes Mode { get; set;  }
    }

}
