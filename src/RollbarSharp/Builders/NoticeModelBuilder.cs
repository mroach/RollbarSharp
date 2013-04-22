using System;
using System.Collections.Generic;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public class NoticeModelBuilder
    {
        public Configuration Configuration { get; protected set; }

        public NoticeModelBuilder(Configuration configuration)
        {
            Configuration = configuration;
        }

        public NoticeModel CreateExceptionNotice(Exception ex)
        {
            var model = Create();
            model.Body = BodyModelBuilder.CreateExceptionBody(ex);
            model.Level = "error"; // sane default
            return model;
        }

        public NoticeModel CreateMessageNotice(string message, IDictionary<string, string> customData)
        {
            var model = Create();
            model.Body = BodyModelBuilder.CreateMessageBody(message, customData);
            model.Level = "info"; // sane default
            return model;
        }

        public NoticeModel Create()
        {
            var model = new NoticeModel();

            model.Environment = Configuration.Environment;
            model.Platform = Configuration.Platform;
            model.Language = Configuration.Language;
            model.Framework = Configuration.Framework;

            model.Timestamp = (ulong)Now();

            model.Request = RequestModelBuilder.CreateFromCurrentRequest();
            model.Server = ServerModelBuilder.CreateFromCurrentRequest();

            model.Notifier = NotifierModelBuilder.CreateFromAssemblyInfo();

            return model;
        }

        private static double Now()
        {
            var epoch = new DateTime(1970, 1, 1);
            return (DateTime.UtcNow - epoch).TotalSeconds;
        }
    }
}
