 using System;
using System.IO;
using DynamicRest;
using Machine.Specifications;
using Provider.Entity.Builder;
using Provider.Entity.Entities;
using System.Linq;
 using Provider.Resource;

namespace Provider.Tests.Resource
{
    class When_bad_path_is_passed
    {

        private static string _path;
        private static PathManager _result;

        private Establish context = () =>
                                        {
                                            _path = @"/foobar";
                                        };

        private Because of = () => _result = new PathManager(_path);

        private It should_have_root_path = () => _result.FindRootPath().ShouldEqual("https://foo");
        private It should_have_path = () => _result.CreatePath().ShouldEqual("https://foo/foobar");

    }

}
