using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DynamicRest;
using Provider.Entity.Builder;
using Provider.Entity.Entities;
using Provider.Resource;
using Token.OAuth2;

namespace Provider.Managers
{
    public class CacheManager
    {
        private readonly string _path;
        private readonly GetToken _token;
        private readonly string _acceptHeader;

        public CacheManager(string path, GetToken token, string acceptHeader)
        {
            _path = path;
            _token = token;
            _acceptHeader = acceptHeader;
        }

        public void UpdateItemCache(TimeSpan time)
        {
            var response = new ResourceFinder(_path, _token, _acceptHeader).Get();
            HttpRuntime.Cache.Insert(_path, response, null, Cache.NoAbsoluteExpiration, time);
        }

        public void UpdateItemCache(TimeSpan time, String path)
        {
            var response = new ResourceFinder(path, _token, _acceptHeader).Get();
            HttpRuntime.Cache.Insert(_path, response, null, Cache.NoAbsoluteExpiration, time);
        }

        public Object GetItemCache()
        {
            return HttpRuntime.Cache.Get(_path);
        }

        public void RemoveItemCache()
        {
            
        }

        public string GetParentCacheLink()
        {
            var response = new ResourceFinder(_path, _token, _acceptHeader).Get();
            IEnumerable<Link> linkAsArray = LinkBuilder.Build(response.Result);
            
            
            return linkAsArray.Single(l => l.Rel == "parent").Href;
        }

    }
}
