using Provider.NavigationProviderParams;

namespace Provider.Resource
{
    public class PathManager : BasePathManager
    {
        public PathManager(string path)
            : base(path, HuddleDocumentLibraryInfo.Host)
        {

        }
    }
}
