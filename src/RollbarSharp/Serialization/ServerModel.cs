using Newtonsoft.Json;

namespace RollbarSharp.Serialization
{
    /// <summary>
    /// Describes the server and code environment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ServerModel
    {
        /// <summary>
        /// The server hostname, e.g. "web1" or "web1.mysite.com"
        /// </summary>
        [JsonProperty("host")]
        public string Host { get; set; }

        /// <summary>
        /// The path to the application code root, not including the final slash
        /// </summary>
        [JsonProperty("root")]
        public string Root { get; set; }

        /// <summary>
        /// The name of the checked out source control branch. Defaults to "master"
        /// </summary>
        [JsonProperty("branch")]
        public string Branch { get; set; }

        /// <summary>
        /// The name of the log file the item was found in
        /// </summary>
        [JsonProperty("log_file")]
        public string LogFile { get; set; }
    }
}