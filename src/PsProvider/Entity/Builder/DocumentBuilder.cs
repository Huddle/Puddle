using PsHuddle.Entity.Entities;

namespace PsHuddle.Entity.Builder
{
    class DocumentBuilder
    {
        public static Document Build(dynamic response)
        {
            return new Document(response.description, response.created, response.updated,
                                response.size, response.version, response.title,
                                LinkBuilder.Build(response));
        }
    }
}
