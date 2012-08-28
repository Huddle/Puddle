using System;

namespace Provider.Entity.Entities
{
    public class Link
    {
        public Link(string rel, String href)
        {
            Rel = rel;
            Href = href;
        }

        public Link()
        {
            
        }
        public string Rel { get;  set; }

        public string Href { get;  set; }

        public override string ToString()
        {
            return string.Format("\n<link rel='{0}' href='{1}'/>", Href, Rel);
        }

        public static EmptyLink Empty()
        {
            return new EmptyLink();
        }

    }

    public class EmptyLink : Link
    {
        public EmptyLink() : base(string.Empty, string.Empty)
        {
        }
    }

}
