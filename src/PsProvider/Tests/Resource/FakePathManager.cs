using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Provider.Resource;

namespace Provider.Tests.Resource
{
    public class PathManager : BasePathManager
    {
        public PathManager(string path)
            : base(path, "foo")
        {

        }
    }
}
