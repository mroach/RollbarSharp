using System.Collections.Generic;
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
        [JsonProperty("trace_chain")]
        public IEnumerable<TraceModel> TraceChain { get; set; }

        public ExceptionBodyModel(IEnumerable<TraceModel> traceChain)
        {
            TraceChain = traceChain;
        }
    }
}