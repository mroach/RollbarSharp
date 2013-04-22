using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RollbarSharp.Builders;
using RollbarSharp.Serialization;

namespace RollbarSharp
{
    public class RollbarClient
    {
        public Configuration Configuration { get; protected set; }

        public NoticeModelBuilder NoticeBuilder { get; protected set; }

        public RollbarClient(Configuration configuration)
        {
            Configuration = configuration;
            NoticeBuilder = new NoticeModelBuilder(Configuration);
        }

        public RollbarClient(string accessToken)
            : this(new Configuration(accessToken))
        {
        }

        public RollbarClient()
            : this(Configuration.CreateFromAppConfig())
        {
        }

        public void SendException(Exception ex)
        {
            var notice = NoticeBuilder.CreateExceptionNotice(ex);
            Send(notice);
        }

        public void SendMessage(string message, IDictionary<string, string> customData = null)
        {
            var notice = NoticeBuilder.CreateMessageNotice(message, customData);
            Send(notice);
        }

        public void Send(NoticeModel notice)
        {
            var payload = JsonConvert.SerializeObject(notice, Formatting.Indented);
            var response = HttpPost(payload);
        }

        private string HttpPost(string payload)
        {
            var request = (HttpWebRequest) WebRequest.Create(Configuration.Endpoint);
            request.ContentType = "application/json";
            request.Method = "POST";

            var payloadBytes = Encoding.GetEncoding(Configuration.Encoding).GetBytes(payload);

            request.ContentLength = payloadBytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(payloadBytes, 0, payloadBytes.Length);
                stream.Close();
            }

            var response = (HttpWebResponse) request.GetResponse();
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
