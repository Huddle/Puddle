using System.Text;

namespace TokenTest.oAuth2
{
    public class RefreshTokenRequest : TokenRequest
    {
        readonly string _refreshToken;

        public RefreshTokenRequest(string clientId, string refreshToken)
            : base(clientId, "foobar", "refresh_token")
        {
            _refreshToken = refreshToken;
        }

        protected override void AddKeys(StringBuilder data)
        {
            data.Append("&refresh_token=" + _refreshToken);
        }
    }
}
