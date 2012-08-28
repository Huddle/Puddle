using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Provider.Entity.Entities;

namespace Provider.Entity.Builder
{
    class WorkSpaceBuilder
    {
        public static Workspace Build(dynamic response)
        {
            try
            {
                return new Workspace(response.type, response.title, LinkBuilder.Build(response.link));
            }
            catch (XmlException ex)
            {
                return null;
            }
        }
    }
}
