using System.IO;
using System.Linq;

namespace PsHuddle.Entity.Entities
{
    public class Workspace : HuddleResourceObject
    {
        public Workspace(string type, string title, Links links)
        {
            Links = links;
            Title = title;
            Type = type;
        }

        public string Type { get; private set; }
        public FileAttributes Mode
        {
            get { return WorkoutMode(); }
        }

        private FileAttributes WorkoutMode()
        {
            return FileAttributes.Directory;
        }

        public string LinkDocumentLibrary
        {
            get { return Links == null ? string.Empty : Links.SingleOrDefault(l => l.Rel == "documentLibrary").Href; }
        }
    }

}
