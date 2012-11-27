using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Machine.Specifications;

namespace Tests.HuddlePsProvider
{
    public abstract class PowershellHostedContext
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