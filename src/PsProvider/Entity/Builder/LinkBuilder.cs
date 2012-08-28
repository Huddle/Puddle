using System.Collections.Generic;
using Provider.Entity.Entities;

namespace Provider.Entity.Builder
{
    class LinkBuilder
    {
        public static Links Build(dynamic response)
        {
            var listLinks = new List<Link>();
            foreach (dynamic li in response.link)
            {
                listLinks.Add(new Link(li.rel, li.href));
            }

            var links = new Links(listLinks);
            return links;
        }
    }
}
