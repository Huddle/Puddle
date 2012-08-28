using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Provider.Entity.Entities;

namespace Provider.Entity.Builder
{
    class DocumentBuilder
    {
        public static Document Build(dynamic response)
        {
            return new Document(response.description, response.created, response.updated,
                                response.size, response.version, response.title,
                                LinkBuilder.Build(response.link));
        }
    }
}
