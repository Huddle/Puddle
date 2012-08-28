using System.Text;

namespace TokenTest.oAuth2
{
    public  class AuthorizationCodeTokenRequest : TokenRequest
    {
        public AuthorizationCodeTokenRequest(string clientId, string authCode, string redirectUri)
            : base(clientId, string.Empty, "authorization_code")
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
    }
}
