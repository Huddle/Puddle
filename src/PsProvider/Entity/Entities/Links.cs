using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Provider.Entity.Entities
{
    public class Links : IEnumerable<Link>
    {
        private readonly IList<Link> _links;

        public Links(IEnumerable<Link> links)
        {
            _links = new List<Link>(links); 
        }

        public void Add(Link link)
        {
            _links.Add(link);
        }

        public IEnumerator<Link> GetEnumerator()
        {
            return _links.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
