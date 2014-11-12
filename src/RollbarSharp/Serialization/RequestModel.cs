using System.Collections.Generic;
using Newtonsoft.Json;

namespace RollbarSharp.Serialization
{
    /// <summary>
    /// Describes the HTTP request that triggered the item being reported.
    /// </summary>
    /// <remarks>The query_string and body parameters have been omitted since they seem redundant to GET and POST</remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class RequestModel
    {
        /// <summary>
        /// The full request URL
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// The request method (e.g. "GET")
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>
        /// Request headers (cookies, user-agent, etc.)
        /// </summary>
        [JsonProperty("headers")]
        public IDictionary<string, string> Headers { get; set; }
        
        /// <summary>
        /// Server-side session data stored in Session object
        /// </summary>
        [JsonProperty("session")]
        public IDictionary<string, string> Session { get; set; }

        /// <summary>
        /// Any routing paramters (e.g. for use with ASP.NET MVC Routes)
        /// </summary>
        [JsonProperty("params")]
        public IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// List of GET parameters (query string params)
        /// </summary>
        [JsonProperty("GET")]
        public IDictionary<string, string> QueryStringParameters { get; set; }

        /// <summary>
        /// List of POST parameters (form-posted data params)
        /// </summary>
        [JsonProperty("POST")]
        public IDictionary<string, string> PostParameters { get; set; }

        /// <summary>
        /// The user's IP address as a string.
        /// </summary>
        [JsonProperty("user_ip")]
        public string UserIp { get; set; }

        /// <summary>
        /// Initialize a new <see cref="RequestModel"/> and initialize Dictionary
        /// properties for easier access.
        /// </summary>
        public RequestModel()
        {
            Headers = new Dictionary<string, string>();
            Parameters = new Dictionary<string, string>();
            QueryStringParameters = new Dictionary<string, string>();
            PostParameters = new Dictionary<string, string>();

            // Ask Rollbar to capture the IP address of the application sending the report
            UserIp = "$remote_ip";
        }
    }
}