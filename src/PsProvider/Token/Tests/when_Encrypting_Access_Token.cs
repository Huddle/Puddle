using System.Security.Cryptography;
using Machine.Specifications;
using TokenTest.oAuth2;

namespace IsolatedStorage.Tests
{
    public class when_Encrypting_Access_Token
    {
        static EncryptData _encryptAccessToken;
        private static byte[] _result;
        private static readonly byte[] JsonBlob =  { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private Establish context = () =>
                                        {
                                            _encryptAccessToken = new EncryptData(JsonBlob);
                                        };

        private Because of = () => _result = _encryptAccessToken.EncryptedData; //act

        private It should_equal_after_round_trip = () => JsonBlob.ShouldEqual(_result.Decrypt()); //assert
        private It should_not_equal_after_encrypt = () => JsonBlob.ShouldNotEqual(_result);
    }

    public static class DecryptExtensions
    {
        public static byte[] Decrypt(this byte[] data)
        {
            return ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
        }
    }

    public static class EncryptExtensions
    {
        public static byte[] Encrypt(this byte[] data)
        {
            return ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
        }
    }
}