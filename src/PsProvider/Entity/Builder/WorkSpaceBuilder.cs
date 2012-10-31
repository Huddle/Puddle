using PsHuddle.Entity.Entities;

namespace PsHuddle.Entity.Builder
{
    class WorkSpaceBuilder
    {
        public static Workspace Build(dynamic response)
        {
            //try
            //{
                return new Workspace(response.type, response.title, LinkBuilder.Build(response));
            //}
            //catch (XmlException ex)
            //{
            //    return null;
            //}
        }
    }
}
