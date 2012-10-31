using System.Collections.Generic;
using PsHuddle.Entity.Entities;

namespace PsHuddle.Entity.Builder
{
    class LinkBuilder
    {
        public static Links Build(dynamic response)
        {
            var listLinks = new List<Link>();

            var responseLinks = response.link;

            foreach (dynamic li in response.link)
            {
                listLinks.Add(new Link(li.rel, li.href));
            }

            var links = new Links(listLinks);
            return links;
        }
    }
}
