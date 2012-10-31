using System;
using System.IO;
using System.Linq;

namespace PsHuddle.Entity.Entities
{

    public class Folder  : HuddleResourceObject
    {
        public Folder(string description, DateTime created, DateTime updated, string title, Links links)
        {
            Links = links;
            Title = title;
            Updated = updated;
            Created = created;
            Description = description;
        }

        public Folder()
        {
            
        }

        public FileAttributes Mode
        {
            get { return WorkoutMode(); }
        }

        private FileAttributes WorkoutMode()
        {
            if (Links.Any(x => x.Rel.Equals("create-folder")))
            {
                return FileAttributes.Directory;
            }

            return FileAttributes.ReadOnly;
        }

        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }

    }
}
