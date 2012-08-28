using System.Security.Cryptography;

namespace TokenTest.oAuth2
{
    public class DecryptData
    {
        private readonly byte[] _byteArray;

        public DecryptData(byte[] byteArray)
        {
            this._byteArray = ProtectedData.Unprotect(byteArray, null, DataProtectionScope.CurrentUser);
        }

        public byte[] DecryptedData
        {
            get { return this._byteArray; }
        }
    }
}
