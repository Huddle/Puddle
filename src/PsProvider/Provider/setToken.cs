using System;
using System.IO;
using System.Management.Automation;
using System.Text;
using Newtonsoft.Json;
using Token.OAuth2;
using Token.Properties;
using TokenTest.oAuth2;

namespace Provider
{
    [Cmdlet("set", "token", DefaultParameterSetName = "default")]
    public class SetToken : PSCmdlet
    {
        private string _clientId;

        private string _clientSecret;
        private readonly string Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "accessToken.txt";
        private static readonly GetAuthCode Code = new GetAuthCode();
        private static readonly TokenClient Client = new TokenClient();
        private static readonly SaveAccessTokenToFile SaveAccessToken = new SaveAccessTokenToFile();
        private string _username;
        private string _password;

        #region Parameters

        /// <summary>
        /// The names of the processes retrieved by the cmdlet.
        /// </summary>


        /// <summary>
        /// Gets or sets the list of process names on which 
        /// the Get-Proc cmdlet will work.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string ClientId
        {
            get { return this._clientId; }
            set { this._clientId = value; }
        }

        [Parameter(Mandatory = true, ParameterSetName = "password")]
        [Parameter(Mandatory = false, ParameterSetName = "default")]
        public string ClientSecret
        {
            get { return this._clientSecret; }
            set { this._clientSecret = value; }
        }

        [Parameter(Mandatory = true, ParameterSetName = "password")]
        public string UserName
        {
            get { return this._username; }
            set { this._username = value; }
        }

        [Parameter(Mandatory = true, ParameterSetName = "password")]
        public string Password
        {
            get { return this._password; }
            set { this._password = value; }
        }

        #endregion Parameters

        protected override void ProcessRecord()
        {
            RootObject token;
            if (File.Exists(Path))
            {
               File.Delete(Path);
            }

            switch (ParameterSetName)
            {
                case "Password":
                    token = GetTokenWithPassword();
                    break;
                default:
                    token = GetTokenWithBrowser();
                    break;
            }

            //encrypt the token
            byte[] unencryptedToken = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(token));
            var encrypt = new EncryptData(unencryptedToken);
            byte[] encryptedToken = encrypt.EncryptedData;

            //save the encrypted token!
            SaveAccessToken.WriteToFile(Path,encryptedToken);

            base.ProcessRecord();
        }

        private RootObject GetTokenWithBrowser()
        {
            RootObject token;
            if (_clientSecret == null)
            {

                var request = new AuthorizationCodeTokenRequest(_clientId, Code.GetCode(_clientId), Settings.Default.RedirectUri);
                token = Client.GetToken(request);
            }
            else
            {
                var request = new AuthorizationCodeTokenRequestWithSecretKey(_clientId, Code.GetCode(_clientId), Settings.Default.RedirectUri, _clientSecret);
                token = Client.GetToken(request);
            }

            return token;
        }


        private RootObject GetTokenWithPassword()
        {
            var request = new TokenRequestWithUserNameAndPassword(_clientId, _clientSecret, _username, _password);
            return Client.GetToken(request);
        }
    }
}
