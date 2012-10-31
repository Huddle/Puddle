 using System;
using System.IO;
using DynamicRest;
using Machine.Specifications;
 using System.Linq;
 using Provider.Resource;

namespace Provider.Tests.Resource
{
    class When_good_path_is_passed
    {

        private static string _path;
        private static PathManager _result;

        private Establish context = () =>
                                        {
                                            _path = @"/files/folders/132700/edit";
                                        };

        private Because of = () => _result = new PathManager(_path);

        private It should_have_root_path = () => _result.FindRootPath().ShouldEqual("https://foo/files/folders/132700");
        private It should_have_path = () => _result.CreatePath().ShouldEqual("https://foo/files/folders/132700/edit");

    }

}
