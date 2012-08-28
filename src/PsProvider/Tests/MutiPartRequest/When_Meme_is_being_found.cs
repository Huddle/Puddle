using System;
using System.IO;
using DynamicRest;
using Machine.Specifications;
using Provider.Entity.Builder;
using Provider.Entity.Entities;
using System.Linq;
using Puddle.Requests.HttpRequests;

namespace Provider.Tests.Entity
{
    class WhenMemeIsBeingFound
    {

        private static Mime mime;
        private static string fileName;
       private static string _result;

        private Establish context = () =>
        {
            fileName = "foo.txt";
            mime = new Mime();
        };

        private Because of = () => _result = mime.LookupMimetypeForFilename(fileName);

        private It should_have_type_text = () => _result.ShouldEqual("text/plain");

    }

}
