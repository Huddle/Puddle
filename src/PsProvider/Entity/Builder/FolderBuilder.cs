using PsHuddle.Entity.Entities;

namespace PsHuddle.Entity.Builder
{
    class FolderBuilder
    {
        public static Folder Build(dynamic response)
        {
            Links links = LinkBuilder.Build(response);

            //try
            //{
            
                return new Folder(string.Empty, response.created,
                                  response.updated, response.title, links);
            //}
            //catch (XmlException ex)
            //{
            //    try
            //    //sometimes its just that the folder doesn't have a description so lets try that else there can be no redemption
            //    {
            //        return new Folder("", response.created,
            //                            response.updated, response.title, links);
            //    }
            //    catch (XmlException e)
            //    {
            //        return null;
            //    }
            //}
        }
    }
}
