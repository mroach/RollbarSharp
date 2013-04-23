using Newtonsoft.Json;

namespace RollbarSharp.Serialization
{
    /// <summary>
    /// Model used when reporting an exception rather than a message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ExceptionBodyModel : BodyModel
    {
        /// <summary>
        /// Exception trace. Includes exception class, message, and backtrace.
        /// </summary>
        [JsonProperty("trace")]
        public TraceModel Trace { get; set; }

        public ExceptionBodyModel(TraceModel trace)
        {
            Trace = trace;
        }
    }
}