using System;
using System.IO;
using DynamicRest;
using Machine.Specifications;
using Provider.Entity.Builder;
using Provider.Entity.Entities;
using System.Linq;

namespace Provider.Tests.Entity
{
    class WheWorkspaceFactoryIsCalled
    {
        #region xml

        private const string xmlResponse = @"<?xml version='1.0' encoding='utf-8'?>
<workspace xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'
  type='shared'
  title='baz'
  description=''>
  <link
    rel='self'
    href='https://api.huddle.dev/workspaces/1371749' />
  <link
    rel='documentLibrary'
    href='https://api.huddle.dev/files/folders/1371754' />
  <link
    rel='changes'
    href='https://api.huddle.dev/files/workspaces/1371749/changes' />
  <link
    rel='workspaceMembers'
    href='https://api.huddle.dev/v2/workspaces/1371749/members' />
  <link
    rel='permissions'
    href='https://api.huddle.dev/files/workspaces/1371749/permissions' />
  <settings>
    <setting
      name='CanSync'
      value='False' />
  </settings>
</workspace>";

        #endregion

        private static Workspace _result;
        private static object result;
        private Establish context = () =>
        {
            var _resultBuilder = new StandardResultBuilder(RestService.Xml);
            result = _resultBuilder.CreateResult(xmlResponse);
        };

        private Because of = () => _result = WorkSpaceBuilder.Build(result);

        private It should_have_the_title_foo = () => _result.Title.ShouldEqual("baz");
        private It should_have_mode = () => _result.Mode.ShouldEqual(FileAttributes.Directory);
        private It should_have_a_self_link =
            () => _result.Links.Single(l => l.Rel == "self").Href.ShouldEqual("https://api.huddle.dev/workspaces/1371749");
    }

}
