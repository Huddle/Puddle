using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;
using DynamicRest;
using DynamicRest.Fluent;
using PsHuddle.Entity.Entities;
using Token.OAuth2;
using TokenTest.oAuth2;

namespace Provider.Resource
{
    public class ResourceUndoDelete
    {
        private readonly GetToken _token;
        private readonly string _acceptHeader = "";
        private readonly string _body = "";
        private readonly dynamic _type;
        private readonly PathManager _pathManager;
        private readonly Dictionary<string, string> customHeaders =  new Dictionary<string, string>();

        public ResourceUndoDelete(string uri, GetToken token, string acceptHeader)
        {
            _pathManager = new PathManager(uri);
            _token = token;
            _acceptHeader = acceptHeader;
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
                .WithBody("")
                .WithAcceptHeader(_acceptHeader);
            var response = restClientBuilder.Build().Put();

            HttpRuntime.Cache.Insert(_pathManager.FindRootPath(), response, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0));
            return response;
        }

        private string SerializeToXml(HuddleResourceObject modifiedItem)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (Object));

            if (_type == "folder")
            {
                serializer = new XmlSerializer(typeof (Folder));
            }
            else if (_type == "workspace")
            {
                serializer = new XmlSerializer(typeof (Workspace));
            }
            else if (_type == "document")
            {
                serializer = new XmlSerializer(typeof (Document));
            }

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, modifiedItem);
                return writer.ToString();
            }
        }
    }
}
