using System;
using System.Text;

namespace TokenTest.oAuth2
{
    public abstract class TokenRequest
    {
        protected TokenRequest(string clientId, string clientSecret, string grantType)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            GrantType = grantType;

            if (String.IsNullOrEmpty(ClientId) || String.IsNullOrEmpty(GrantType))
            {
                throw new TokenRequestException("Invalid clientId: " + ClientId + " or grantType: " + GrantType);
            }
        }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }

        /// <summary>
        /// Supports integration testing by allowing the client to request a token with a specific lifetime.
        /// If this value is missing, or GREATER than the maximum allowed value, it is ignored.
        /// </summary>
        public int? ExpirySeconds { get; set; }

        public string ToForm()
        {
            var data = new StringBuilder();

            data.Append("client_id=" + ClientId);

            if (string.IsNullOrEmpty(ClientSecret) == false)
                data.Append("&client_secret=" + ClientSecret);

            data.Append("&grant_type=" + GrantType);
            if (null != ExpirySeconds)
            {
                data.Append("&ExpirySeconds=" + ExpirySeconds);
            }

            AddKeys(data);
            return data.ToString();
        }

        protected abstract void AddKeys(StringBuilder data);
    }

    public class TokenRequestException : Exception
    {
        public TokenRequestException(string message)
            : base(message) { }
    }
}
