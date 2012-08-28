using System.IO;
using Huddle.Sync.Utility.MimeType;

namespace Puddle.Requests.HttpRequests
{
    public class Mime
    {
        public const string DefaultMimeType = "application/octet-stream";

        public string LookupMimetypeForFilename(string filename)
        {
            return LookupMimeTypeForExtension(Path.GetExtension(filename));
        }

        private string LookupMimeTypeForExtension(string extension)
        {
            return LookupMimeTypeForExtensionFromApacheList(extension);
        }

        private string LookupMimeTypeForExtensionFromApacheList(string extension)
        {
            string mimeType = DefaultMimeType;

            if (!string.IsNullOrEmpty(extension))
            {
                extension = extension.Trim().ToLower().Replace(".", string.Empty);

                string tryResult;
                ApacheMimeTypeList.MimeTypes.TryGetValue(extension, out tryResult);

                if (!string.IsNullOrEmpty(tryResult))
                {
                    mimeType = tryResult;
                }
            }

            return mimeType;
        }
    }
}
