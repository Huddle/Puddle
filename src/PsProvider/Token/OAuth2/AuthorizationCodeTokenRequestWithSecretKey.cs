using System.Text;

namespace TokenTest.oAuth2
{
    public class AuthorizationCodeTokenRequestWithSecretKey : TokenRequest
    {
        public AuthorizationCodeTokenRequestWithSecretKey(string clientId, string authCode, string redirectUri, string secretKey)
            : base(clientId, secretKey, "authorization_code")
        {
            Code = authCode;
            Redirect = redirectUri;
        }

        protected override void AddKeys(StringBuilder data)
        {
            data.Append("&code=" + Code);
            data.Append("&redirect_uri=" + Redirect);
        }

        public string Redirect { get; set; }

        public string Code { get; set; }

        public string SecretKey { get; set; }
    }
}
