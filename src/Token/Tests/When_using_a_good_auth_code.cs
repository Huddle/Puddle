using Machine.Specifications;
using Token.OAuth2;
using Token.Properties;
using TokenTest.oAuth2;

namespace IsolatedStorage.Tests
{
    public class When_using_a_good_auth_code
    {
        private static GetAuthCode authcode;
        static AuthorizationCodeTokenRequest request;
        static TokenClient client;
        static RootObject Result;

         Establish context = () =>
             // fetch an auth code
                                 {
                                     authcode = new GetAuthCode();

                                     request = new AuthorizationCodeTokenRequest(Settings.Default.ClientId, "nooooooooooo", Settings.Default.RedirectUri);
                                     client = new TokenClient();
                                 };

        private Because we_fetch_the_token = () => Result = client.GetToken(request);

        private It should_have_content = () => Result.ShouldNotBeNull();


    }
}