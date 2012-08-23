﻿using System.Management.Automation;
using System.IO.IsolatedStorage;
using System.Text;
using IsolatedStorage;
using IsolatedStorage.Properties;
using IsolatedStorage.oAuth2;

namespace Puddle
{
    [Cmdlet("set", "token", DefaultParameterSetName = "default")]
    public class SetToken : PSCmdlet
    {
        private string _clientId;

        private string _clientSecret;
        private const string Path = "@accessToken.txt";
        private static readonly IsolatedStorageFile IsolatedFile = IsolatedStorageFile.GetMachineStoreForAssembly();
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
            TokenResponse token;
            if (IsolatedFile.FileExists(Path))
            {
                IsolatedFile.DeleteFile(Path);
            }

            switch (ParameterSetName)
            {
                case "Password":
                    token = GetTokenWithPassword();
                    WriteObject("Access_token: " + token.AccessToken + ",Expires_in: "+ token.ExpirySeconds + ", Refresh_token: " + token.RefreshToken);
                    break;
                default:
                    token = GetTokenWithBrowser();
                    WriteObject("Access_token: " + token.AccessToken + ",Expires_in: "+ token.ExpirySeconds + ", Refresh_token: " + token.RefreshToken);
                    break;
            }

            //encrypt the token
            byte[] unencryptedToken = Encoding.UTF8.GetBytes("{Access_token: " + token.AccessToken + ", Expires_in: " + token.ExpirySeconds + ", Refresh_token: " + token.RefreshToken + "}");
            var encrypt = new EncryptData(unencryptedToken);
            byte[] encryptedToken = encrypt.EncryptedData;

            //save the encrypted token!
            SaveAccessToken.WriteToFile(Path, encryptedToken, IsolatedStorageFile.GetMachineStoreForAssembly());

            base.ProcessRecord();
        }

        private TokenResponse GetTokenWithBrowser()
        {
            TokenResponse token;
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


        private TokenResponse GetTokenWithPassword()
        {
            var request = new TokenRequestWithUserNameAndPassword(_clientId, _clientSecret, _username, _password);
            return Client.GetToken(request);
        }
    }
}
