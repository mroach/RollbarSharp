using Newtonsoft.Json;

namespace RollbarSharp.Serialization
{
    /// <summary>
    /// Object describing the user affected by the item. 'id' is required.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PersonModel
    {
        /// <summary>
        /// User ID of the affected user. A string up to 40 characters. Required.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Affected user's username. A string up to 255 characters. Optional.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Affected user's email address. A string up to 255 characters. Optional.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}