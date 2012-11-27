using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Machine.Specifications;

namespace Tests.HuddlePsProvider
{
    public class When_checking_valid_path : PowershellHostedContext
    {
        Establish context = () => Execute<ProviderInfo>("new-psdrive -psprovider Huddle.Ps.Provider -name huddle -root '' -Scope Global");
        Because we_check_for_valid_path = () => Execute<ProviderInfo>("new-psdrive -psprovider Huddle.Ps.Provider -name huddle -root '' -Scope Global");
        It should_create_a_huddle_provider = () => Execute<ProviderInfo>("get-psprovider").Any(pi => pi.Name == "Huddle.Ps.Provider").ShouldBeTrue();
        It should_create_a_huddle_drive = () => Execute<PSDriveInfo>("get-psdrive").Any(psd => psd.Name == "huddle").ShouldBeTrue();
    }
}