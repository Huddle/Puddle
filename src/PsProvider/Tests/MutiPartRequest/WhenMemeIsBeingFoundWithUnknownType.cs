using Machine.Specifications;
using Puddle.Requests.HttpRequests;

namespace Provider.Tests.Entity
{
    class WhenMemeIsBeingFoundWithUnknownType
    {

        private static Mime mime;
        private static string fileName;
       private static string _result;

        private Establish context = () =>
        {
            fileName = "foo.foo";
            mime = new Mime();
        };

        private Because of = () => _result = mime.LookupMimetypeForFilename(fileName);

        private It should_have_type_octet = () => _result.ShouldEqual("application/octet-stream");

    }

}
