using System;
using System.IO;
using DynamicRest;
using Machine.Specifications;
using System.Linq;
using PsHuddle.Entity.Builder;
using PsHuddle.Entity.Entities;

namespace Provider.Tests.Entity
{
    class WheDocumentFactoryIsCalled
    {
        #region xml

        private const string xmlResponse = @"<?xml version='1.0' encoding='utf-8'?>
<document xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
  title='test'
  description=''>
  <link
    rel='self'
    href='https://api.huddle.dev/files/documents/1371775' />
  <link
    rel='version-history'
    href='https://api.huddle.dev/files/documents/1371775/versions' />
  <link
    rel='comments'
    href='https://api.huddle.dev/files/documents/1371775/comments'
    count='0' />
  <link
    rel='approvals'
    href='https://api.huddle.dev/files/documents/1371775/approvals' />
  <link
    rel='permissions'
    href='https://api.huddle.dev/files/folders/1364653/permissions' />
  <link
    rel='share'
    href='https://api.huddle.dev/files/documents/1371775/share' />
  <link
    rel='audittrail'
    href='https://api.huddle.dev/files/documents/1371775/audittrail' />
  <link
    rel='content'
    href='https://api.huddle.dev/files/documents/115428/content'
    type=''
    title='test.py' />
  <link
    rel='parent'
    href='https://api.huddle.dev/files/folders/1364653'
    title='zidsal' />
  <link
    rel='edit'
    href='https://api.huddle.dev/files/documents/1371775/edit' />
  <link
    rel='create-version'
    href='https://api.huddle.dev/files/documents/1371775/version' />
  <link
    rel='lock'
    href='https://api.huddle.dev/files/documents/1371775/lock' />
  <link
    rel='delete'
    href='https://api.huddle.dev/files/documents/1371775' />
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
  <actor
    name='adam flax'
    email='adam.flax@huddle.net'
    rel='updated-by'>
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
  <version>1</version>
  <size>12</size>
  <updated>2012-08-23T09:08:57Z</updated>
  <created>2012-08-23T09:08:56Z</created>
  <processingStatus>Complete</processingStatus>
  <views>0</views>
</document>";

        #endregion

        private static Document _result;
        private static object result;
        private Establish context = () =>
        {
            var _resultBuilder = new StandardResultBuilder(RestService.Xml);
            result = _resultBuilder.CreateResult(xmlResponse);
        };

        private Because of = () => _result = DocumentBuilder.Build(result);

        private It should_have_the_created_timestamp_now = () => _result.Created.ShouldEqual(DateTime.Parse("2012-08-23T09:08:56Z"));
        private It should_have_the_title_foo = () => _result.Title.ShouldEqual("test");
        private It should_have_mode = () => _result.Mode.ShouldEqual(FileAttributes.Normal);

        private It should_have_a_self_link =
            () => _result.Links.Single(l => l.Rel == "self").Href.ShouldEqual("https://api.huddle.dev/files/documents/1371775");
    }

}
