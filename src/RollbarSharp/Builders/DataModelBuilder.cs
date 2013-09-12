using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public class DataModelBuilder
    {
        public Configuration Configuration { get; protected set; }

        public DataModelBuilder()
            :this(Configuration.CreateFromAppConfig())
        {
        }

        public DataModelBuilder(Configuration configuration)
        {
            Configuration = configuration;
        }

        public DataModel CreateExceptionNotice(Exception ex, string message = null, string level = "error")
        {
            var body = BodyModelBuilder.CreateExceptionBody(ex);
            var model = Create(level, body);
            
            // if the exception has a fingerprint property, copy it to the notice
            if (!string.IsNullOrEmpty(body.Trace.Exception.Fingerprint))
                model.Fingerprint = FingerprintHash(body.Trace.Exception.Fingerprint);

            if (body.Trace.Exception.Data != null)
                model.Custom["exception_data"] = body.Trace.Exception.Data;

            model.Title = message;

            return model;
        }

        public DataModel CreateMessageNotice(string message, string level = "info", IDictionary<string, object> customData = null)
        {
            return Create(level, BodyModelBuilder.CreateMessageBody(message, customData));
        }

        /// <summary>
        /// Create the best stub of a request that we can using the message level and body
        /// </summary>
        /// <param name="level"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        protected DataModel Create(string level, BodyModel body)
        {
            var model = new DataModel(level, body);
            
            model.Environment = Configuration.Environment;
            model.Platform = Configuration.Platform;
            model.Language = Configuration.Language;
            model.Framework = Configuration.Framework;

            model.Timestamp = (ulong)Now();

            model.Notifier = NotifierModelBuilder.CreateFromAssemblyInfo();

            model.Request = RequestModelBuilder.CreateFromCurrentRequest();
            model.Server = ServerModelBuilder.CreateFromCurrentRequest();
            model.Server.GitSha = Configuration.GitSha;
            model.Person = PersonModelBuilder.CreateFromCurrentRequest();
            
            return model;
        }

        /// <summary>
        /// Current UTC date time as a UNIX timestamp
        /// </summary>
        /// <returns></returns>
        private static double Now()
        {
            var epoch = new DateTime(1970, 1, 1);
            return (DateTime.UtcNow - epoch).TotalSeconds;
        }

        public static string FingerprintHash(params object[] fields)
        {
            return FingerprintHash(string.Join(",", fields));
        }

        /// <summary>
        /// To make sure fingerprints are the correct length and don't
        /// contain any problematic characters, SHA1 the fingerprint.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string FingerprintHash(string data)
        {
            using (var sha = new SHA1Managed())
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                var hash = sha.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }
    }
}
