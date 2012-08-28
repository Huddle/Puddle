using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;
using DynamicRest;
using DynamicRest.Fluent;
using Newtonsoft.Json;
using Provider.Entity.Builder;
using Provider.Entity.Entities;
using Token.OAuth2;

namespace Provider.Resource
{
    public class ResourceModifier
    {
        private readonly GetToken _token;
        private readonly string _acceptHeader = "";
        private readonly string _body = "";
        private readonly PathManager _pathManager;
        
        public ResourceModifier(string uri, GetToken token, string acceptHeader, HuddleResourceObject modifiedItem)
        {
            _pathManager = new PathManager(uri);
            _token = token;
            _acceptHeader = acceptHeader;
            _body = SerializeToXml(modifiedItem);
        }

        public RestOperation Put()
        {
            if (HttpRuntime.Cache[_pathManager.FindRootPath()] != null)
           {
                HttpRuntime.Cache.Remove(_pathManager.FindRootPath());
           }

            var restClientBuilder = new RestClientBuilder()
                .WithUri(_pathManager.CreatePath())
                .WithOAuth2Token(_token.GetAccessToken())
                .WithBody(_body)
                .WithAcceptHeader(_acceptHeader)
                .WithContentType("application/vnd.huddle.data+json");
            var response = restClientBuilder.Build().Put();


            //this is fine and dandy but then we have to update any cache that holds info about this i.e thhe parent folder cache
            var restClientBuilderParent = new RestClientBuilder()
                .WithUri(_pathManager.FindRootPath())
                .WithOAuth2Token(_token.GetAccessToken())
                .WithAcceptHeader(_acceptHeader);
            
           var parentResponse = restClientBuilderParent.Build().get();

           IEnumerable<Link> linkAsArray = LinkBuilder.Build(parentResponse.Result);
           var parentLink = linkAsArray.Single(l => l.Rel == "parent");

           if (HttpRuntime.Cache[parentLink.Href] != null)
           {
               HttpRuntime.Cache.Remove(parentLink.Href);
               HttpRuntime.Cache.Insert(parentLink.Href, parentResponse, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0));
           }

            return response;
        }

        private string SerializeToXml(HuddleResourceObject modifiedItem)
        {
            return JsonConvert.SerializeObject(modifiedItem);
        }
    }
}
