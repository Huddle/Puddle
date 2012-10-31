using Provider.NavigationProviderParams;
using PsHuddle.NavigationProviderParams;

namespace Provider.Resource
{
    public class PathManager : BasePathManager
    {
        public PathManager(string path)
            : base(path, HuddleDriveInfo.Host)
        {

        }
    }
}
