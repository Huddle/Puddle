
using System.IO.IsolatedStorage;
using Machine.Specifications;
using TokenTest.oAuth2;

namespace IsolatedStorage.Tests
{
    public class when_Access_Token_Is_Read_From_File
    {
        private static readonly byte[] JsonBlob =  { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private static IsolatedStorageFile _isolatedFile;
        private const string Path = "@accessToken.txt";
        private static SaveAccessTokenToFile _saveAccessTokenToFile;
        private static ReadAccessTokenFromFile _readAccessTokenFromFile;
        private static byte[] _result; 

        private Establish context = () =>
                                        {
                                            _isolatedFile =  IsolatedStorageFile.GetMachineStoreForAssembly();
                                            _readAccessTokenFromFile = new ReadAccessTokenFromFile();
                                        };

        //private Because of = () => _result = _readAccessTokenFromFile.ReadFile(Path, _isolatedFile);//act

        private It should_match_content_saved_to_file = () => _result.ShouldEqual(JsonBlob);

    }


}