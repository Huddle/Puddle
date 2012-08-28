using System.Text;

namespace TokenTest.oAuth2
{
    public class TokenRequestWithUserNameAndPassword : TokenRequest
    {
        public TokenRequestWithUserNameAndPassword(string clientId, string clientSecret, string username, string password)
            : base(clientId, clientSecret, "password")
        {
            Username = username;
            Password = password;
        }

        protected override void AddKeys(StringBuilder data)
        {
            data.Append("&username=" + Username);
            data.Append("&password=" + Password);
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}