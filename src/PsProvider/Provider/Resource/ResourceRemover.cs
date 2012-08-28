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
    class ResourceRemover
    {
        private readonly GetToken _token;
        private readonly string _acceptHeader = "";
        private readonly PathManager _pathManager;

        public ResourceRemover(string uri, GetToken token, string acceptHeader)
        {
            _pathManager = new PathManager(uri);
            _token = token;
            _acceptHeader = acceptHeader;
        }

        public RestOperation Delete()
        {
            if (HttpRuntime.Cache[_pathManager.FindRootPath()] != null)
            {
                HttpRuntime.Cache.Remove(_pathManager.FindRootPath());
            }

            //this is fine and dandy but then we have to update any cache that holds info about this i.e thhe parent folder cache
            var restClientBuilderParent = new RestClientBuilder()
                .WithUri(_pathManager.FindRootPath())
                .WithOAuth2Token(_token.GetAccessToken())
                .WithAcceptHeader(_acceptHeader);
            var parentResponse = restClientBuilderParent.Build().get();

            IEnumerable<Link> linkAsArray = LinkBuilder.Build(parentResponse.Result.link);
            var parentLink = linkAsArray.Single(l => l.Rel == "parent");

            if (HttpRuntime.Cache[parentLink.Href] != null)
            {
                HttpRuntime.Cache.Remove(parentLink.Href);
            }

            var restClientBuilder = new RestClientBuilder()
                .WithUri(_pathManager.CreatePath())
                .WithOAuth2Token(_token.GetAccessToken())
                .WithAcceptHeader(_acceptHeader);
            var response = restClientBuilder.Build().Delete();

            return response;
        }
    }
}
