using System;

namespace Puddle
{
    public class Folder
    {
        public Folder(string description, string title, DateTime createdDt, DateTime updatedDt)
        {
            Description = description;
            Title = title;
        }

        public string Description { get; private set; }
        public string Title { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime Updated { get; private set; }
    }
}