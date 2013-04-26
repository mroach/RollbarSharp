using System.Collections.Generic;
using Newtonsoft.Json;

namespace RollbarSharp.Serialization
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DataModel
    {
        /// <summary>
        /// Running environment. E.g. production, staging, development
        /// </summary>
        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("body")]
        public BodyModel Body { get; set; }

        /// <summary>
        /// UNIX timestamp of the error event
        /// </summary>
        [JsonProperty("timestamp")]
        public ulong Timestamp { get; set; }

        /// <summary>
        /// Severity. One of: critical, error, warning, info, debug
        /// </summary>
        [JsonProperty("level")]
        public string Level { get; set; }

        /// <summary>
        /// Code platform. E.g. browser, flash, heroku, google-app-engine
        /// </summary>
        [JsonProperty("platform")]
        public string Platform { get; set; }
        
        /// <summary>
        /// Code language. Will be defaulted to "csharp"
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Name of the code's framework. Will be defaulted to ".net"
        /// </summary>
        [JsonProperty("framework")]
        public string Framework { get; set; }

        [JsonProperty("request")]
        public RequestModel Request { get; set; }

        [JsonProperty("person")]
        public PersonModel Person { get; set; }

        [JsonProperty("server")]
        public ServerModel Server { get; set; }

        /// <summary>
        /// Arbitrary JSON object describing the client environment.
        /// </summary>
        [JsonProperty("client")]
        public object Client { get; set; }

        /// <summary>
        /// An object containing arbitrary custom data
        /// </summary>
        [JsonProperty("custom")]
        public IDictionary<string, object> Custom { get; set; }

        [JsonProperty("notifier")]
        public NotifierModel Notifier { get; set; }

        /// <summary>
        /// A string up to 40 characters long that identifies the "fingerprint" of this item.
        /// Items with the same fingerprint are grouped together in Rollbar.
        /// Rollbar will automatically compute a fingerprint, but you can pass one explicitly if you want to override it.
        /// </summary>
        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        /// <summary>
        /// A string up to 255 characters that will be used as the item title in the Rollbar UI.
        /// Rollbar will automatically compute a title, but you can pass one explicitly if you want to override it.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        public DataModel(string level, BodyModel body)
        {
            Level = level;
            Body = body;
            Custom = new Dictionary<string, object>();
        }
    }
}