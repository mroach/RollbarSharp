using Newtonsoft.Json;

namespace RollbarSharp.Serialization
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FrameModel
    {
        /// <summary>
        /// Name of the file including path
        /// </summary>
        [JsonProperty("filename")]
        public string FileName { get; set; }

        /// <summary>
        /// Code line number.
        /// </summary>
        [JsonProperty("lineno")]
        public int LineNumber { get; set; }

        /// <summary>
        /// Name of the method
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>
        /// The line of code executing
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// An object containing pre and post code context, as lists of lines of code.
        /// Doesn't apply to .NET
        /// </summary>
        [JsonProperty("context")]
        public object Context { get; set; }

        public FrameModel(string fileName, int lineNumber = 0, string method = null)
        {
            FileName = fileName;
            LineNumber = lineNumber;
            Method = method;
        }
        
        public override string ToString()
        {
            var s = Code ?? Method;

            if (!string.IsNullOrEmpty(FileName))
                s += " in " + FileName;

            if (LineNumber > 0)
                s += ":line " + LineNumber;

            return s;
        }
    }
}