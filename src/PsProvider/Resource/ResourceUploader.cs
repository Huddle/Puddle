using System;
using System.Web;
using System.Web.Caching;
using DynamicRest;
using DynamicRest.Fluent;
using Puddle.Requests.HttpRequests;
using Token.OAuth2;

namespace Provider.Resource
{
    class ResourceUploader
    {
        private readonly GetToken _token;
        private readonly string _acceptHeader = "";
        private readonly string _fileToUpload;
        private readonly PathManager _pathManager;

        public ResourceUploader(string uri, GetToken token, string acceptHeader, string fileToUpload)
        {
            _pathManager = new PathManager(uri);
            _fileToUpload = fileToUpload;
            _token = token;
            _acceptHeader = acceptHeader;
            _fileToUpload = fileToUpload;
        }

        public RestOperation SendMutiPartRequest()
        {

            string boundary = Guid.NewGuid().ToString();
            var data = new GetMutiPartData(_fileToUpload);

            var restClientBuilder = new RestClientBuilder()
                .WithUri(_pathManager.CreatePath())
                .WithContentType("multipart/form-data; boundary=" + boundary)
                .WithOAuth2Token(_token.GetAccessToken())
                .WithBody(data.ConstructMutiPartData(boundary))
                .WithAcceptHeader(_acceptHeader);
            var response = restClientBuilder.Build().Put();

            if (HttpRuntime.Cache[_pathManager.FindRootPath()] != null)
            {
                HttpRuntime.Cache.Remove(_pathManager.FindRootPath());
            }

            HttpRuntime.Cache.Insert(_pathManager.FindRootPath(), response, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0));

            return response;
        }
    }
}
