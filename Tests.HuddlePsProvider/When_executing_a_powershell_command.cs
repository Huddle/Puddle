using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using Machine.Specifications;

namespace Tests.HuddlePsProvider
{
    public class When_executing_a_powershell_command_to_retrieve_processes : PowershellHostedContext
    {
        Establish context = () => {   };
        Because we_get_the_running_processes = () => result = Execute<Process>("get-process");
        It should_count_greater_than_10 = () => result.Count().ShouldBeGreaterThan(10);

        private static IEnumerable<Process> result;
    }
}
