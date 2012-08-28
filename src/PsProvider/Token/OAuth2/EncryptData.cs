using System.Security.Cryptography;

namespace TokenTest.oAuth2
{
    public class EncryptData
    {
        private readonly byte[] _byteArray;
        public EncryptData(byte[] byteArray)
        {
            var encryptedBlob = ProtectedData.Protect(byteArray, null, DataProtectionScope.CurrentUser);
            this._byteArray = encryptedBlob;
        }

        public byte[] EncryptedData
        {
            get { return this._byteArray; }
        }
    }
}
