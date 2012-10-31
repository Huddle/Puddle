using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Machine.Specifications;

namespace Tests.HuddlePsProvider
{
    public class When_executing_a_powershell_command : PowershellHostedContext
    {
        Establish context = () => {   };
        Because we_create_the_version_file = () => result = Execute<ProviderInfo>("get-process");
        It should_create_the_version_file_correctly = () => "0.0.0.0".ShouldEqual("0.0.0.0");

        private static IEnumerable<ProviderInfo> result;
    }
    public class When_adding_a_huddle_psdrive : PowershellHostedContext
    {
        static string _versionFilePath;

        Establish context = () =>{};

        Because we_create_the_version_file = () => Execute<ProviderInfo>("new-psdrive -psprovider Huddle.Ps.Provider -name huddle -root '' -Scope Global");
        It should_create_a_huddle_provider = () =>
                                                 {
                                                     IEnumerable<ProviderInfo> providerInfos = Execute<ProviderInfo>("get-psprovider");
                                                     providerInfos.Any(pi => pi.Name == "Huddle.Ps.Provider").ShouldBeTrue();
                                                 };
        It should_create_a_huddle_drive = () => Execute<PSDriveInfo>("get-psdrive").Any(psd => psd.Name == "huddle").ShouldBeTrue();
    }

    public class PowershellHostedContext
    {
        private static Runspace _runspace;

        Establish context = () =>
        {
            InitialSessionState initial = InitialSessionState.CreateDefault();
            //initial.ImportPSModule(new[] { @"Huddle-Ps-Provider.psd1" });
            initial.ImportPSModule(new[] { @"D:\Dev\git\Puddle\Tests.HuddlePsProvider\bin\Debug\Huddle.Ps.Provider.dll" });
            _runspace = RunspaceFactory.CreateRunspace(initial);
            
            _runspace.Open();
        };

        private Cleanup the_pipeline = () => _runspace.Dispose();

        protected static IEnumerable<T> Execute<T>(string cmd)
        {
            var invoker = new RunspaceInvoke(_runspace);
            Collection<PSObject> results = invoker.Invoke(cmd);
            return results.Select(r => r.BaseObject).Cast<T>();
        }
    }


}
