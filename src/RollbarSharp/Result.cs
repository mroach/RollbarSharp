using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RollbarSharp
{
    /// <summary>
    /// Result of a post to the Rollbar API.
    /// The API communicates general status using HTTP status codes which are mapped to status descriptions
    /// </summary>
    public class Result
    {
        /// <summary>
        /// HTTP status code from the Rollbar endpoint
        /// </summary>
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// Raw response string. Usually JSON format.
        /// </summary>
        public string RawResponse { get; set; }

        /// <summary>
        /// Description of the status
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// User-provided parameter from Send* functions
        /// </summary>
        public object UserParam { get; set; }

        /// <summary>
        /// Successful or not
        /// </summary>
        public bool IsSuccess { get { return HttpStatusCode == 200; } }

        public string Description
        {
            get
            {
                if (HttpStatusCode == 200) return "Success";
                if (HttpStatusCode == 400) return "Bad or missing request data";
                if (HttpStatusCode == 403) return "Access denied. Check your access token";
                if (HttpStatusCode == 422) return "Unprocessable payload. Payload contains semantic errors.";
                if (HttpStatusCode == 429) return "Too many requests. Rate limit exceeded.";
                if (HttpStatusCode == 500) return "Internal server error.";
                return "Unknown";
            }
        }

        public Result(int httpStatusCode, string rawResponse, object userParam)
        {
            HttpStatusCode = httpStatusCode;
            RawResponse = rawResponse;
            UserParam = userParam;
            TryParseResponse(rawResponse);
        }

        /// <summary>
        /// Responses are usually (always?) in JSON format
        /// </summary>
        /// <param name="response"></param>
        protected void TryParseResponse(string response)
        {
            if (string.IsNullOrEmpty(response))
                return;

            try
            {
                var hash = JsonConvert.DeserializeObject<JObject>(response);
                Message = hash["message"].Value<string>();
            }
            catch
            {
            }
        }

        /// <summary>
        /// HTTP Status code + status description + message if not successful
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var desc = HttpStatusCode + ": " + Description;

            if (!IsSuccess)
                desc += ": " + (Message ?? RawResponse);

            return desc;
        }
    }
}