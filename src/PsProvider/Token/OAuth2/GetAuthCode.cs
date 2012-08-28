using System;
using Token.Properties;

namespace TokenTest.oAuth2
{
    public class GetAuthCode
    {
        public String GetCode(String _ClientId)
        {

            System.Console.WriteLine("Please Enter the Auth code into the console!");
            string url = string.Format("{0}request?response_type=code&client_id={1}&redirect_uri={2}", Settings.Default.HuddleAuthServer, _ClientId, Settings.Default.RedirectUri);
            System.Diagnostics.Process.Start(url);

            String authCode = System.Console.ReadLine();

            if(!IsValidAuthCode(authCode))
            {
                throw new AuthCodeException("Invalid Auth Code!", authCode);
            }

            return authCode;
        }

        public class AuthCodeException : Exception
        {
            private readonly string _authCode;

            public AuthCodeException(string message,string authCode)
                : base(message)
            {
                _authCode = authCode;
            }

            public string AuthCode
            {
                get { return _authCode; }
            }

        }

        public Boolean IsValidAuthCode(String authCode)
        {
            //TODO: ADD ME PLEASE
            return true;
        }
    }


}
