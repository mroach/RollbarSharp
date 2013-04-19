using System.Collections.Generic;
using Newtonsoft.Json;

namespace RollbarSharp.Serialization
{
    public abstract class BodyModel
    {
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ExceptionBodyModel : BodyModel
    {
        [JsonProperty("trace")]
        public TraceModel Trace { get; set; }

        public ExceptionBodyModel(TraceModel trace)
        {
            Trace = trace;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class MessageBodyModel : BodyModel
    {
        public string Message { get; set; }

        public IDictionary<string, string> CustomData { get; set; }

        [JsonProperty("message")]
        internal IDictionary<string, string> Serialized
        {
            get
            {
                var result = new Dictionary<string, string>(CustomData);
                result["message"] = Message;
                return result;
            }
        }

        public MessageBodyModel(string message, IDictionary<string, string> customData = null)
        {
            Message = message;
            CustomData = customData ?? new Dictionary<string, string>();
        }
    }
}