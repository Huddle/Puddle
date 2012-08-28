using System;
using System.IO;

namespace TokenTest.oAuth2
{
    public class SaveAccessTokenToFile
    {
        public void WriteToFile(String path, byte[] jsonBlob)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (var writer = new BinaryWriter(File.Open(path,FileMode.Create)))
            {
                writer.Write(jsonBlob);
            }
        }
    }
}