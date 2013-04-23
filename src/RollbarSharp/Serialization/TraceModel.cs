using Newtonsoft.Json;

namespace RollbarSharp.Serialization
{
    /// <summary>
    /// Container for the exception detils as well as the backtrace frames
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TraceModel
    {
        /// <summary>
        /// Description of the exception itself. Exception class and exception message.
        /// </summary>
        [JsonProperty("exception")]
        public ExceptionModel Exception { get; set; }

        /// <summary>
        /// Stack trace
        /// </summary>
        [JsonProperty("frames")]
        public FrameModel[] Frames { get; set; }

        public TraceModel(ExceptionModel ex, FrameModel[] frames)
        {
            Exception = ex;
            Frames = frames;
        }
    }
}