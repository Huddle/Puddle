using System;
using System.Collections.Generic;
using System.IO;

namespace Provider.Entity.Entities
{

    public class Folder  : HuddleResourceObject
    {
        [NonSerialized]
        private FileAttributes _mode;

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
            get { return _mode; }
        }

        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }

    }
}
