using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Token.Properties;
using TokenTest.oAuth2;

namespace Token.OAuth2
{
    //todo swap to deseralization
    public class GetToken
    {
        private readonly string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "accessToken.txt";
        private readonly string _tokenFromFile;
        private RootObject _token;
        private readonly string Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "accessToken.txt";
        private static readonly SaveAccessTokenToFile SaveAccessToken = new SaveAccessTokenToFile();

        public GetToken()
        {
            if (!File.Exists(_path))
            {
                throw new FileNotFoundException("No Access Token found! set one with the cmdlet set-token");
            }

            var readToken = new ReadAccessTokenFromFile();
            var encryptedToken = readToken.ReadFile(_path);
            var decrypt = new DecryptData(encryptedToken);
            _tokenFromFile = Encoding.UTF8.GetString(decrypt.DecryptedData);

            _token = JsonConvert.DeserializeObject<RootObject>(_tokenFromFile);
        }


        public string GetRefreshToken()
        {
            return _token.refresh_token;
        }

        public string GetAccessToken()
        {
            if(_token.IsExpired())
            {
                var request = new RefreshTokenRequest(Settings.Default.ClientId,GetRefreshToken());
                //encrypt the token
                byte[] unencryptedToken = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
                var encrypt = new EncryptData(unencryptedToken);
                byte[] encryptedToken = encrypt.EncryptedData;

                //save the encrypted token!
                SaveAccessToken.WriteToFile(Path, encryptedToken);
                //reset the acccess token to the new 1
                _token = JsonConvert.DeserializeObject<RootObject>(Encoding.UTF8.GetString(unencryptedToken));
            }
            return _token.access_token;
        }

    }
}
