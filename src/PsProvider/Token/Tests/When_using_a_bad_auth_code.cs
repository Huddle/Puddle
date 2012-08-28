using System;
using Machine.Specifications;
using TokenTest.oAuth2;

namespace IsolatedStorage.Tests
{
    public class When_using_a_bad_auth_code
    {
         Establish context = () =>
             // fetch an auth code
                                 {
                                     request = new AuthorizationCodeTokenRequest("foo", "code", "badger");
                                     client = new TokenClient();

                                 };

        private Because we_fetch_the_token = () => Response = Catch.Exception(() => client.GetToken(request));
        It should_fail = () => Response.ShouldBeOfType(typeof(OAuthException));
        private It should_contain_message = () => Response.ShouldNotBeNull();


        static AuthorizationCodeTokenRequest request;
        static TokenClient client;
        static Exception Response;
    }
}