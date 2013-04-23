using NLog;
using NLog.Common;
using NLog.Targets;

namespace RollbarSharp.NLog
{
    public class RollbarTarget : TargetWithLayout
    {
        protected RollbarClient RollbarClient { get; set; }

        public RollbarTarget()
        {
            RollbarClient = new RollbarClient();
            RollbarClient.RequestCompleted += RollbarClientRequestCompleted;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var level = ConvertLogLevel(logEvent.Level);
            
            if (logEvent.Exception != null)
            {
                RollbarClient.SendException(logEvent.Exception, logEvent.FormattedMessage, level);
                return;
            }
            
            RollbarClient.SendMessage(logEvent.FormattedMessage, level);
        }

        private static void RollbarClientRequestCompleted(object source, RequestCompletedEventArgs args)
        {
            if (args.Result.IsSuccess)
            {
                InternalLogger.Debug("Request was successful: " + args.Result.Message);
                return;
            }

            InternalLogger.Warn("Request failed: " + args.Result);
        }

        /// <summary>
        /// Convert the NLog level to a level string understood by Rollbar
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static string ConvertLogLevel(LogLevel level)
        {
            if (level == LogLevel.Fatal) return "critical";
            if (level == LogLevel.Error) return "error";
            if (level == LogLevel.Warn) return "warning";
            if (level == LogLevel.Info) return "info";
            if (level == LogLevel.Debug) return "debug";
            return "debug";
        }
    }
}
