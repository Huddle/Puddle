using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Huddle.Ps.Provider;
using Machine.Specifications;

namespace Tests.HuddlePsProvider
{
    public class When_adding_a_huddle_psdrive : PowershellHostedContext
    {
        private const string host = "api.huddle.com";
        Establish context = () => { };
        Because we_create_the_version_file = () => Execute<ProviderInfo>("new-psdrive -psprovider Huddle.Ps.Provider -name huddle -root '' -Scope Global -Host {0}");
        It should_create_a_huddle_provider = () => Execute<ProviderInfo>("get-psprovider").Any(pi => pi.Name == "Huddle.Ps.Provider").ShouldBeTrue();
        It should_create_a_huddle_drive = () => Execute<PSDriveInfo>("get-psdrive").Any(psd => psd.Name == "huddle").ShouldBeTrue();
    }
    public class When_adding_and_removing_huddle_psdrive : PowershellHostedContext
    {
        private const string host = "api.huddle.com";
        Establish context = () => Execute<ProviderInfo>("new-psdrive -psprovider Huddle.Ps.Provider -name huddle -root '' -Scope Global -Host {0}");
        Because we_create_the_version_file = () => Execute<ProviderInfo>("remove-psdrive -psprovider Huddle.Ps.Provider -name huddle");
        It should_remove_the_huddle_provider = () => Execute<ProviderInfo>("get-psprovider").Any(pi => pi.Name == "Huddle.Ps.Provider").ShouldBeTrue();
    }
    public class When_adding_a_huddle_psdrive_With_parameters : PowershellHostedContext
    {
        private const string host = "api.huddle.com";
        Establish context = () => { };
        Because we_create_the_version_file = () => Execute<ProviderInfo>(string.Format("new-psdrive -psprovider Huddle.Ps.Provider -name huddle -root '' -Scope Global -Host {0}", host));
        private It should_create_a_huddle_drive_with_expected_host = () =>
                                                      {
                                                          var provs = Execute<PSDriveInfo>("get-psdrive");
                                                          HuddlePSDriveInfo huddleProvider = provs.SingleOrDefault(psd => psd.Name == "huddle") as HuddlePSDriveInfo;
                                                          huddleProvider.ShouldNotBeNull();
                                                          huddleProvider.Host.ShouldEqual(host);
                                                      };
    }
    public class When_adding_a_huddle_psdrive_without_host : PowershellHostedContext
    {
        private static Exception exception;
        Establish context = () => { };
        Because we_cfreate_a_ps_driove_without_a_host = () => exception = Catch.Exception( () => Execute<ProviderInfo>(string.Format("new-psdrive -psprovider Huddle.Ps.Provider -name huddle -root '' -Scope Global")));
        It should_raise_parm_binding_exception = () => exception.ShouldBeOfType<ParameterBindingException>();
    }
}