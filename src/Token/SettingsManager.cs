  using Token.Properties;

namespace Token
{
    public static class SettingsManager
    {
        public static string RedirectUri
        {
            get { return Settings.Default.RedirectUri; }
        }
    }
}