using System;
using System.IO;

namespace Provider.Entity.Entities
{
    public class Document : HuddleResourceObject
    {
        public Document(string description, DateTime created, DateTime updated, int size, string version, string title, Links links)
        {
            Links = links;
            Title = title;
            Version = version;
            Size = size;
            Updated = updated;
            Created = created;
            Description = description;
        }

        public Document()
        {
            
        }

        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }
        public int Size { get; private set;  }
        public string Version { get; private set; }

        public FileAttributes Mode { get;set; }
    }
}
