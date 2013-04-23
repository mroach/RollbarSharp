using Newtonsoft.Json;

namespace RollbarSharp.Serialization
{
    /// <summary>
    /// Wrapper for the whole payload. This is the access token as one item
    /// and the whole notice request as another
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PayloadModel
    {
        /// <summary>
        /// Access token
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Body of the request
        /// </summary>
        [JsonProperty("data")]
        public DataModel Data { get; set; }

        public PayloadModel(string accessToken, DataModel data)
        {
            AccessToken = accessToken;
            Data = data;
        }
    }
}
