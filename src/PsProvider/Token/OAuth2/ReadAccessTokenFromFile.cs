using System;
using System.IO;

namespace TokenTest.oAuth2
{
    public class ReadAccessTokenFromFile
    {        
        public byte[] ReadFile(String path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            byte[] data;
            using (var reader = new BinaryReader(File.Open(path,FileMode.Open)))
            {
                data = reader.ReadBytes((int) reader.BaseStream.Length);
            }

            return data;
        }
    }
}