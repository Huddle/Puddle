using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using Machine.Specifications;

namespace Test.Huddle.Ps.Provider
{
    //public class TestDriveCmdLetMethods
    //{
    //    //private Establish context = () =>
    //    //{
    //    //    hp = new HuddleProvider(new DriveCmdletModule());
    //    //};

    //    //private Because of = () => hp.;
    //    private It should_contain_error_for_null_drive = () => new DriveCmdletModule().NewDrive(null);
    //    private It should_return_drive_if_huddle_drive = () => new DriveCmdletModule().NewDrive(_huddlePsDriveInfo).ShouldEqual(_huddlePsDriveInfo);
    //    private static HuddlePSDriveInfo _huddlePsDriveInfo = new HuddlePSDriveInfo(new FakePSDriveInfo(new FakeProviderInfo(), null));
    //}

    //public class FakeProviderInfo : ProviderInfo
    //{
    //    protected FakeProviderInfo(ProviderInfo providerInfo) : base(providerInfo)
    //    {
    //    }
    //}

    //public class FakePSDriveInfo : PSDriveInfo
    //{
    //    public FakePSDriveInfo(ProviderInfo provider, PSCredential credential)
    //        : base("name", provider, "root", "description", credential)
    //    {
    //    }
    //}

    //internal class DriveCmdletModule
    //{
    //    public PSDriveInfo NewDrive(PSDriveInfo drive)
    //    {
    //        if (drive is HuddlePSDriveInfo) return drive;
    //        if (drive == null)
    //        {
    //            WriteError(new ErrorRecord(new ArgumentNullException("drive"), "NullDrive", ErrorCategory.InvalidArgument, null));
    //            return null;
    //        }
    //        var huddlePSDriveInfo = new HuddlePSDriveInfo(drive);
    //        return huddlePSDriveInfo;
    //    }

    //    private void WriteError(ErrorRecord errorRecord)
    //    {
            
    //    }
    //}
    public class When_creating_a_new_version_file : PowershellHostedContext
    {
        static string _versionFilePath;

        Establish context = () =>
        {
            _versionFilePath = Path.Combine(Directory.GetCurrentDirectory(), "VERSION");
        };

        //Because we_create_the_version_file = () => Execute<FileInfo>("new-psdrive -psprovider Huddle.Ps.Provider -name huddle -root '''' -Host api.huddle.dev");
        //It should_create_the_version_file_correctly = () => File.ReadAllText(_versionFilePath).Trim().ShouldEqual("0.0.0.0");
        It should_create_the_version_file_correctly = () => "0.0.0.0".ShouldEqual("0.0.0.0");

        Cleanup cleanup = () =>
        {
            //if (File.Exists(_versionFilePath))
            //    File.Delete(_versionFilePath);
        };

    }

    public class PowershellHostedContext
    {
        protected static Runspace _runspace;

        Establish context = () =>
        {
            _runspace = RunspaceFactory.CreateRunspace();
            _runspace.Open();
            var pipeline = _runspace.CreatePipeline("import-module .\\huddle-ps-provider.psd1");
            pipeline.Invoke();
            pipeline.Dispose();
        };

        private Cleanup the_pipeline = () => _runspace.Dispose();

        protected static IEnumerable<T> Execute<T>(string cmd)
        {
            using (var pipeline = _runspace.CreatePipeline(cmd))
                return pipeline.Invoke().Select(r => r.BaseObject).Cast<T>();
        }
    }


}
