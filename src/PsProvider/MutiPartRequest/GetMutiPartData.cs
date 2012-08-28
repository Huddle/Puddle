using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Puddle.Requests.HttpRequests
{
    //yes I'm really doing it like this =-( leave me alone I'm just an intern
    class GetMutiPartData
    {
        private String filePath = "";
        public GetMutiPartData(String filePath)
        {
            this.filePath = filePath;
        }

        public string ConstructMutiPartData(String boundary)
        {
            Mime mime = new Mime();
            const string name = "\"filename\"";
            string fileName = "\"" + Path.GetFileName(filePath) + "\"";
            string type = mime.LookupMimetypeForFilename(filePath);
            string data = Encoding.UTF8.GetString(ReadFile());

            var builder = new StringBuilder();
            //keep it as simple as possible for now because mutipart form data is #!@*?
            builder.Append("--" + boundary);
            builder.Append(Environment.NewLine);
            builder.Append(String.Format("Content-Disposition: form-data;name={0}; filename={1}", name, fileName));
            builder.Append(Environment.NewLine);
            builder.Append("Content-Type:" + type);
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append(data);
            builder.Append(Environment.NewLine);
            builder.Append("--" + boundary + "--");

            return builder.ToString();
        }

        private byte[] ReadFile()
        {
            byte[] buffer;
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                var length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }
    }
}
