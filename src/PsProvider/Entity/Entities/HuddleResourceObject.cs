using System.Linq;

namespace PsHuddle.Entity.Entities
{
    public class HuddleResourceObject
    {
        public Links Links { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }

        public string LinkSelf
        {
            get { return LinkFor("self"); }
        }

        public bool IsContainer
        {
            get { return !(this is Document); }
        }

        public string LinkFor(string rel)
        {
            return Links==null ? string.Empty : Links.SingleOrDefault(l => l.Rel== rel).Href; 
        }
    }
}
