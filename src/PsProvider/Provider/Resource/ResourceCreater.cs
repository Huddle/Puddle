using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DynamicRest;
using DynamicRest.Fluent;
using Provider.Entity.Builder;
using Provider.Entity.Entities;
using Token.OAuth2;
using TokenTest.oAuth2;

namespace Provider.Resource
{
    public class ResourceCreater
    {
        private readonly GetToken _token;
        private readonly string _acceptHeader = "";
        private readonly string _body = "";
        private readonly PathManager _pathManager;

        public ResourceCreater(string uri, GetToken token, string acceptHeader, string body)
        {
            _pathManager = new PathManager(uri);
            _token = token;
            _acceptHeader = acceptHeader;
            _body = body;
        }

        public RestOperation Post()
        {
            if (HttpRuntime.Cache[_pathManager.FindRootPath()] != null)
            {
                HttpRuntime.Cache.Remove(_pathManager.FindRootPath());
            }

            var restClientBuilder = new RestClientBuilder()
                .WithUri(_pathManager.CreatePath())
                .WithOAuth2Token(_token.GetAccessToken())
                .WithBody(_body)
                .WithAcceptHeader(_acceptHeader);
            var response = restClientBuilder.Build().Post();

            //to cache it we need to know where we  made the object so lets get its self link

            IEnumerable<Link> linkAsArray = LinkBuilder.Build(response.Result.link);
            var selfLink = linkAsArray.Single(l => l.Rel == "self");
            var parentLink = linkAsArray.Single(l => l.Rel == "parent");

            HttpRuntime.Cache.Insert(selfLink.Href, response, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0));

            if (HttpRuntime.Cache[parentLink.Href] != null)
            {
                HttpRuntime.Cache.Remove(parentLink.Href);
            }

            return response;
        }
    }
}
