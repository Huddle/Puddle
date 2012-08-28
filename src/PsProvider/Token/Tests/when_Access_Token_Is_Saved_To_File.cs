using System;
using System.IO.IsolatedStorage;
using Machine.Specifications;
using TokenTest.oAuth2;

namespace IsolatedStorage.Tests
{
    public class when_Access_Token_Is_Saved_To_File
    {
        private static byte[] jsonBlob =  { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private static IsolatedStorageFile _isolatedFile;
        private static SaveAccessTokenToFile _saveAccessToken;
        private static bool _result;
        private static string path = "@accessToken.txt";
        private static DateTime _timeOfFileMade;

        private Establish context = () =>
                                        {
                                            _isolatedFile =  IsolatedStorageFile.GetMachineStoreForAssembly();
                                            _saveAccessToken = new SaveAccessTokenToFile();
                                        };

        //private Because of = () => _saveAccessToken.WriteToFile(path, jsonBlob, _isolatedFile); //act

        private It should_have_been_made = () => _isolatedFile.FileExists(path).ShouldBeTrue();
        private It should_have_content = () => _isolatedFile.UsedSize.ShouldBeGreaterThan(0);

    }



}