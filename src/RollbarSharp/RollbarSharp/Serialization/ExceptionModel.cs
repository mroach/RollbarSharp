using Newtonsoft.Json;

namespace RollbarSharp.Serialization
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExceptionModel
    {
        /// <summary>
        /// The class name of the exception (or some other string describing the error class)
        /// </summary>
        /// <example>ArgumentException</example>
        [JsonProperty("class")]
        public string Class { get; set; }

        /// <summary>
        /// The exception message (should not be prefixed with the class name)
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// If the exception defined a 'fingerprint' item in its Data property,
        /// it will be stored here for use on the notice's <see cref="NoticeModel.Fingerprint"/>
        /// </summary>
        public string Fingerprint { get; set; }

        public ExceptionModel(string exceptionClass, string message)
        {
            Class = exceptionClass;
            Message = message;
        }
    }
}