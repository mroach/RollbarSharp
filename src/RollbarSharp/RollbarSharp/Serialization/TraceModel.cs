using Newtonsoft.Json;

namespace RollbarSharp.Serialization
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TraceModel
    {
        [JsonProperty("exception")]
        public ExceptionModel Exception { get; set; }

        [JsonProperty("frames")]
        public FrameModel[] Frames { get; set; }

        public TraceModel(ExceptionModel ex, FrameModel[] frames)
        {
            Exception = ex;
            Frames = frames;
        }
    }
}