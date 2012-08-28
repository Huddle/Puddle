using System.Collections.Generic;
using System.Xml;
using Provider.Entity.Entities;

namespace Provider.Entity.Builder
{
    class FolderBuilder
    {
        public static Folder Build(dynamic response)
        {
            Links links = LinkBuilder.Build(response.link);

            try
            {
                return new Folder(response.description, response.created,
                                  response.updated, response.title, links);
            }
            catch (XmlException ex)
            {
                try
                //sometimes its just that the folder doesn't have a description so lets try that else there can be no redemption
                {
                    return new Folder("", response.created,
                                        response.updated, response.title, links);
                }
                catch (XmlException e)
                {
                    return null;
                }
            }
        }
    }
}
