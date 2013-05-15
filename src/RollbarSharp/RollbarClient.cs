using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RollbarSharp.Builders;
using RollbarSharp.Serialization;

namespace RollbarSharp
{
    /// <summary>
    /// The Rollbar client. This is where applications will interact
    /// with Rollbar. There shouldn't be any need for them to deal
    /// with any objects aside from this
    /// </summary>
    public class RollbarClient
    {
        /// <summary>
        /// Signature for the handler fired when the request is complete
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public delegate void RequestCompletedEventHandler(object source, RequestCompletedEventArgs args);

        public delegate void RequestSendingEventHandler(object source, RequestStartingEventArgs args);

        public Configuration Configuration { get; protected set; }

        /// <summary>
        /// Builds Rollbar requests from <see cref="Exception"/>s or text messages
        /// </summary>
        /// <remarks>This only builds the body of the request, not the whole notice payload</remarks>
        public DataModelBuilder NoticeBuilder { get; protected set; }

        /// <summary>
        /// Fires just before sending the final JSON payload to Rollbar
        /// </summary>
        public event RequestSendingEventHandler RequestStarting;

        /// <summary>
        /// Fires when we've received a response from Rollbar
        /// </summary>
        public event RequestCompletedEventHandler RequestCompleted;

        public RollbarClient(Configuration configuration)
        {
            Configuration = configuration;
            NoticeBuilder = new DataModelBuilder(Configuration);
        }

        /// <summary>
        /// Creates a new RollbarClient using the given access token
        /// and all default <see cref="Configuration"/> values
        /// </summary>
        /// <param name="accessToken"></param>
        public RollbarClient(string accessToken)
            : this(new Configuration(accessToken))
        {
        }

        /// <summary>
        /// Creates a new RollbarClient using configuration values from app/web.config
        /// </summary>
        public RollbarClient()
            : this(Configuration.CreateFromAppConfig())
        {
        }

        /// <summary>
        /// Sends an exception using the "critical" level
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        public void SendCriticalException(Exception ex, string title = null)
        {
            SendException(ex, title, "critical");
        }

        /// <summary>
        /// Sends an exception using the "error" level
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        public void SendErrorException(Exception ex, string title = null)
        {
            SendException(ex, title, "error");
        }

        /// <summary>
        /// Sents an exception using the "warning" level
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        public void SendWarningException(Exception ex, string title = null)
        {
            SendException(ex, title, "warning");
        }

        /// <summary>
        /// Sends the given <see cref="Exception"/> to Rollbar including
        /// the stack trace. 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="level">Default is "error". "critical" and "warning" may also make sense to use.</param>
        public void SendException(Exception ex, string title = null, string level = "error")
        {
            var notice = NoticeBuilder.CreateExceptionNotice(ex, title, level);
            Send(notice);
        }

        /// <summary>
        /// Sends a text notice using the "critical" level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="customData"></param>
        public void SendCriticalMessage(string message, IDictionary<string, object> customData = null)
        {
            SendMessage(message, "critical", customData);
        }

        /// <summary>
        /// Sents a text notice using the "error" level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="customData"></param>
        public void SendErrorMessage(string message, IDictionary<string, object> customData = null)
        {
            SendMessage(message, "error", customData);
        }

        /// <summary>
        /// Sends a text notice using the "warning" level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="customData"></param>
        public void SendWarningMessage(string message, IDictionary<string, object> customData = null)
        {
            SendMessage(message, "warning", customData);
        }

        /// <summary>
        /// Sends a text notice using the "info" level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="customData"></param>
        public void SendInfoMessage(string message, IDictionary<string, object> customData = null)
        {
            SendMessage(message, "info", customData);
        }

        /// <summary>
        /// Sends a text notice using the "debug" level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="customData"></param>
        public void SendDebugMessage(string message, IDictionary<string, object> customData = null)
        {
            SendMessage(message, "debug", customData);
        }

        /// <summary>
        /// Sents a text notice using the given level of severity
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="customData"></param>
        public void SendMessage(string message, string level, IDictionary<string, object> customData = null)
        {
            var notice = NoticeBuilder.CreateMessageNotice(message, level, customData);
            Send(notice);
        }

        public void Send(DataModel data)
        {
            var payload = new PayloadModel(Configuration.AccessToken, data);
            HttpPost(payload);
        }

        /// <summary>
        /// Serialize the given object for transmission
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, Configuration.JsonSettings);
        }

        protected void HttpPost(PayloadModel payload)
        {
            var payloadString = Serialize(payload);
            HttpPost(payloadString);
        }

        protected void HttpPost(string payload)
        {
            Task.Factory.StartNew(() => HttpPostAsync(payload));
        }

        protected void HttpPostAsync(string payload)
        {
            // convert the json payload to bytes for transmission
            var payloadBytes = Encoding.GetEncoding(Configuration.Encoding).GetBytes(payload);

            var request = (HttpWebRequest) WebRequest.Create(Configuration.Endpoint);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.ContentLength = payloadBytes.Length;

            OnRequestStarting(payload);

            // we need to wrap GetRequestStream() in a try block
            // if the endpoint is unreachable, that exception gets thrown here
            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(payloadBytes, 0, payloadBytes.Length);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                OnRequestCompleted(new Result(0, ex.Message));
                return;
            }

            // attempt to parse the response. wrap GetResponse() in a try block
            // since WebRequest throws exceptions for HTTP error status codes
            WebResponse response;

            try
            {
                response = request.GetResponse();
            }
            catch (WebException ex)
            {
                OnRequestCompleted(ex.Response);
                return;
            }
            catch (Exception ex)
            {
                OnRequestCompleted(new Result(0, ex.Message));
                return;
            }

            OnRequestCompleted(response);
        }

        protected void OnRequestStarting(string payload)
        {
            if (RequestStarting == null)
                return;

            RequestStarting(this, new RequestStartingEventArgs(payload));
        }

        protected void OnRequestCompleted(WebResponse response)
        {
            var responseCode = (int) ((HttpWebResponse) response).StatusCode;
            string responseText;

            using (var stream = response.GetResponseStream())
            {
                if (stream == null)
                    responseText = string.Empty;
                else
                {
                    using (var reader = new StreamReader(stream))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
            }
            
            var result = new Result(responseCode, responseText);
            OnRequestCompleted(result);
        }

        protected void OnRequestCompleted(Result result)
        {
            if (RequestCompleted == null)
                return;

            var args = new RequestCompletedEventArgs(result);
            RequestCompleted(this, args);
        }
    }
}
