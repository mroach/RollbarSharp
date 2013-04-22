using System.Configuration;

namespace RollbarSharp
{
    public class Configuration
    {
        public static string DefaultEndpoint = "https://api.rollbar.com/api/1/item/";
        public static string DefaultEnvironment = "production";
        public static string DefaultPlatform = ".NET";
        public static string DefaultLanguage = "csharp";
        public static string Encoding = "utf-8";

        public string Endpoint { get; set; }

        public string AccessToken { get; set; }

        public string Environment { get; set; }

        public string Platform { get; set; }

        public string Language { get; set; }

        public string Framework { get; set; }

        public Configuration(string accessToken)
        {
            Endpoint = DefaultEndpoint;
            AccessToken = accessToken;
            Environment = DefaultEnvironment;
            Platform = DefaultPlatform;
            Language = DefaultLanguage;
        }

        public static Configuration CreateFromAppConfig()
        {
            var token = GetSetting("Rollbar.AccessToken");

            if (string.IsNullOrEmpty(token))
                throw new ConfigurationErrorsException("Missing access token at Rollbar.AccessToken");

            return new Configuration(token)
                       {
                           Endpoint = GetSetting("Rollbar.Endpoint", DefaultEndpoint),
                           Environment = GetSetting("Rollbar.Environment", DefaultEnvironment),
                           Platform = GetSetting("Rollbar.Platform", DefaultPlatform),
                           Language = GetSetting("Rollbar.CodeLanguage", DefaultLanguage),
                           Framework = ".NET"
                       };
        }

        internal static string GetSetting(string name, string fallback = null)
        {
            return ConfigurationManager.AppSettings[name] ?? fallback;
        }
    }
}
