using System;
using System.Net;
using DynamicRest.Fluent;
using Token.OAuth2;
using Token.Properties;

namespace TokenTest.oAuth2
{
    public class TokenClient
    {
        public RootObject GetToken(TokenRequest request)
        {
            var client = new RestClientBuilder()
                .WithAcceptHeader("application/json")
                .WithContentType("application/x-www-form-urlencoded")
                .WithBody(request.ToForm())
                .WithUri(Settings.Default.HuddleAuthServer + Settings.Default.HuddleTokenEndPoint)
                .Build().Post();



            if (client.StatusCode == HttpStatusCode.BadRequest)
            {
 
            }

            var token = new RootObject();
            token.access_token = client.Result.access_token;
            token.expires_in = DateTime.Now.AddSeconds(client.Result.expires_in);
            token.refresh_token = client.Result.refresh_token;

            return token;
        }
    }

    public class OAuthException : Exception
    {
        private readonly string _errorKey;
        private readonly string _errorUri;

        public OAuthException(string errorKey, string message, string errorUri)
            : base(message)
        {
            _errorKey = errorKey;
            _errorUri = errorUri;
        }

        public string ErrorKey
        {
            get { return _errorKey; }
        }

        public string ErrorUri
        {
            get { return _errorUri; }
        }
    }
}