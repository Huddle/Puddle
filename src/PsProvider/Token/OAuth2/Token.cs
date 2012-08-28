
//do not touch used for seralisation BEWARE OF DRAGONS please someone think about the dragons

using System;

namespace Token.OAuth2
{
    public class RootObject
    {
        public string access_token { get; set; }
        public DateTime expires_in { get; set; }
        public string refresh_token { get; set; }


        public Boolean IsExpired()
        {
            return DateTime.UtcNow.CompareTo(expires_in) > 0;
        }
    }
}
