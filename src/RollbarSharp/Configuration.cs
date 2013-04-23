using System.Configuration;
using Newtonsoft.Json;

namespace RollbarSharp
{
    public class Configuration
    {
        /// <summary>
        /// Default endpoint URL for posting notices (default: https://api.rollbar.com/api/1/item/)
        /// </summary>
        public static string DefaultEndpoint = "https://api.rollbar.com/api/1/item/";

        /// <summary>
        /// Default environment name used in notices. (default: production)
        /// </summary>
        public static string DefaultEnvironment = "production";

        /// <summary>
        /// Default language name used in notices. (default: csharp)
        /// </summary>
        public static string DefaultLanguage = "csharp";
        
        /// <summary>
        /// Encoding to use when communicating with the Rollbar endpoint (default: utf-8)
        /// </summary>
        public static string Encoding = "utf-8";


        /// <summary>
        /// URL for the Rollbar API
        /// Setting: Rollbar.Endpoint
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The server-side access token for your application in Rollbar.
        /// Also known as the post_server_item key
        /// 
        /// Setting: Rollbar.AccessToken
        /// Default: None. You have to set this.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Name of the environment this app is running in. Usually "production" or "staging"
        /// 
        /// Setting: Rollbar.Environment
        /// Default: production
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Platform running the code. E.g. Windows or IIS.
        /// 
        /// Setting: Rollbar.Platform
        /// Default: <see cref="System.Environment.OSVersion"/>
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Code language. Defaults to csharp.
        /// 
        /// Setting: Rollbar.Language
        /// Default: csharp
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// .NET Framework version
        /// 
        /// Setting: Rollbar.Framework
        /// Default: ".NET" + <see cref="System.Environment.Version"/>
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Settings used to serialize the payload to JSON when posting to Rollbar
        /// </summary>
        public JsonSerializerSettings JsonSettings
        {
            get
            {
                return new JsonSerializerSettings
                           {
                               Formatting = Formatting.Indented,
                               NullValueHandling = NullValueHandling.Ignore
                           };
            }
        }

        public Configuration(string accessToken)
        {
            Endpoint = DefaultEndpoint;
            AccessToken = accessToken;
            Environment = DefaultEnvironment;
            Platform = System.Environment.OSVersion.ToString();
            Framework = ".NET " + System.Environment.Version;
            Language = DefaultLanguage;
        }

        /// <summary>
        /// Creates a <see cref="Configuration"/>, reading values from App/Web.config
        /// Rollbar.AccessToken
        /// Rollbar.Endpoint
        /// Rollbar.Environment
        /// Rollbar.Platform
        /// Rollbar.CodeLanguage
        /// Rollbar.Framework
        /// </summary>
        /// <returns></returns>
        public static Configuration CreateFromAppConfig()
        {
            var token = GetSetting("Rollbar.AccessToken");

            if (string.IsNullOrEmpty(token))
                throw new ConfigurationErrorsException("Missing access token at Rollbar.AccessToken");

            var conf = new Configuration(token);

            conf.Endpoint = GetSetting("Rollbar.Endpoint") ?? conf.Endpoint;
            conf.Environment = GetSetting("Rollbar.Environment") ?? conf.Environment;
            conf.Platform = GetSetting("Rollbar.Platform") ?? conf.Platform;
            conf.Language = GetSetting("Rollbar.CodeLanguage") ?? conf.Language;
            conf.Framework = GetSetting("Rolllbar.Framework") ?? conf.Framework;

            return conf;
        }

        protected static string GetSetting(string name, string fallback = null)
        {
            var setting = ConfigurationManager.AppSettings[name];
            return string.IsNullOrEmpty(setting) ? fallback : setting;
        }
    }
}
