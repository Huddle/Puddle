using System;
using System.IO;
using DynamicRest;
using Machine.Specifications;
using Provider.Entity.Builder;
using Provider.Entity.Entities;
using System.Linq;

namespace Provider.Tests.Entity
{
    class WhenFolderFactoryIsCalled
    {
        #region xml

        private const string xmlResponse = @"<?xml version='1.0' encoding='utf-8'?>
<folder xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
  title='foo'
  description='bar'>
  <link
    rel='create-document'
    href='https://api.huddle.dev/files/folders/1371724/documents' />
  <link
    rel='create-folder'
    href='https://api.huddle.dev/files/folders/1371724' />
  <link
    rel='delete'
    href='https://api.huddle.dev/files/folders/1371724' />
  <link
    rel='self'
    href='https://api.huddle.dev/files/folders/1371724' />
  <link
    rel='permissions'
    href='https://api.huddle.dev/files/folders/1371724/permissions' />
  <link
    rel='workspace-summary'
    href='https://api.huddle.dev/workspaces/1327034' />
  <link
    rel='edit'
    href='https://api.huddle.dev/files/folders/1371724/edit' />
  <link
    rel='editPermissions'
    href='https://api.huddle.dev/files/folders/1371724/permissions' />
  <link
    rel='parent'
    href='https://api.huddle.dev/files/folders/1327039' />
  <updated>2012-08-23T10:15:32Z</updated>
  <created>2012-08-23T08:54:24Z</created>
  <actor
    name='adam flax'
    email='adam.flax@huddle.net'
    rel='owner'>
    <link
      rel='self'
      href='https://api.huddle.dev/users/1237965' />
    <link
      rel='avatar'
      href='https://api.huddle.dev/files/users/1237965/avatar'
      type='image/jpeg' />
    <link
      rel='alternate'
      href='https://my.huddle.dev/user/adam.flax'
      type='text/html' />
  </actor>
  <folders />
  <documents />
</folder>";

        #endregion

        private static Folder _result;
        private static object result;
        private Establish context = () =>
        {
            var _resultBuilder = new StandardResultBuilder(RestService.Xml);
            result = _resultBuilder.CreateResult(xmlResponse);
        };

        private Because of = () => _result = FolderBuilder.Build(result);

        private It should_have_the_created_timestamp_now = () => _result.Created.ShouldEqual(DateTime.Parse("2012-08-23T08:54:24Z"));
        private It should_have_the_title_foo = () => _result.Title.ShouldEqual("foo");
        private It should_have_mode = () => _result.Mode.ShouldEqual(FileAttributes.Directory);

        private It should_have_a_self_link =
            () => _result.Links.Single(l => l.Rel == "self").Href.ShouldEqual("https://api.huddle.dev/files/folders/1371724");
    }

}
