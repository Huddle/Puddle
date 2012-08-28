using System;
using System.Net;
using System.Web;
using System.Web.Caching;
using DynamicRest;
using DynamicRest.Fluent;
using Token.OAuth2;
using TokenTest.oAuth2;

namespace Provider.Resource
{
    public class ResourceFinder
    {
        private readonly GetToken _token;
        private readonly string _acceptHeader = "";
        private readonly PathManager _pathManager;

        public ResourceFinder(string uri, GetToken token, string acceptHeader)
        {
            _pathManager = new PathManager(uri);
            _token = token;
            _acceptHeader = acceptHeader;
        }

        public RestOperation Get()
        {
            if (HttpRuntime.Cache[_pathManager.FindRootPath()] != null)
            {
                return (RestOperation)HttpRuntime.Cache[_pathManager.FindRootPath()];
            }

            var restClientBuilder = new RestClientBuilder()
                .WithUri(_pathManager.CreatePath())
                .WithOAuth2Token(_token.GetAccessToken())
                .WithAcceptHeader(_acceptHeader);
            var response = restClientBuilder.Build().Get();

            if (!String.IsNullOrEmpty(response.GetResponseHeader(HttpResponseHeader.Location)))
            {
                var redirect = (response.GetResponseHeader(HttpResponseHeader.Location));
                var redirectFinder = new ResourceFinder(redirect, _token, _acceptHeader);
                return redirectFinder.Get();
            }

            HttpRuntime.Cache.Insert(_pathManager.FindRootPath(), response, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0));
            return response;
        }
    }
}
