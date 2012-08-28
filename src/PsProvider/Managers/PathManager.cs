using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Provider.NavigationProviderParams;

namespace Provider
{
    class PathManager
    {
        private const string https = "https://";
        private string _host;
        private string _path;

        public PathManager(string path)
        {
            _host = HuddleDocumentLibraryInfo.Host;
            _path = path;
        }

        public string CreatePath()
        {

            if (_path == "" || _path == "entry")
            {
                return https + _host + "entry";
            }


            if (_path.Contains(https + _host))
            {
                return _path;
            }
            else
            {
                return https + _host + _path;
            }
        }

        public string FindRootPath()
        {

            if(_path == "" || _path == "entry")
            {
                return _path;
            }

            var lastSlash = _path.LastIndexOf('/');

            if (lastSlash < 0)
            {
                return _path;
            }

            if(IsNumber(_path.Substring(lastSlash+1)))
            {
                return _path.Contains(https + _host) ? _path : https + _host + _path;
            }

            return _path.Contains(https + _host) ? _path.Substring(0, lastSlash) : https + _host + _path.Substring(0, lastSlash);
        }

        private Boolean IsNumber(string text)
        {
            double number;
            return double.TryParse(text, out number);
        }
    }
}
