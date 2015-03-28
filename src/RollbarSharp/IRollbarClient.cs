using System;
using System.Collections.Generic;
using RollbarSharp.Builders;
using RollbarSharp.Serialization;

namespace RollbarSharp
{
    public interface IRollbarClient
    {
        Configuration Configuration { get; }

        /// <summary>
        /// Builds Rollbar requests from <see cref="Exception"/>s or text messages
        /// </summary>
        /// <remarks>This only builds the body of the request, not the whole notice payload</remarks>
        DataModelBuilder NoticeBuilder { get; }

        /// <summary>
        /// Fires just before sending the final JSON payload to Rollbar
        /// </summary>
        event RollbarClient.RequestSendingEventHandler RequestStarting;

        /// <summary>
        /// Fires when we've received a response from Rollbar
        /// </summary>
        event RollbarClient.RequestCompletedEventHandler RequestCompleted;

        /// <summary>
        /// Sends an exception using the "critical" level
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="modelAction"></param>
        void SendCriticalException(Exception ex, string title = null, Action<DataModel> modelAction = null, object userParam = null);

        /// <summary>
        /// Sends an exception using the "error" level
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="modelAction"></param>
        void SendErrorException(Exception ex, string title = null, Action<DataModel> modelAction = null, object userParam = null);

        /// <summary>
        /// Sents an exception using the "warning" level
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="modelAction"></param>
        void SendWarningException(Exception ex, string title = null, Action<DataModel> modelAction = null, object userParam = null);

        /// <summary>
        /// Sends the given <see cref="Exception"/> to Rollbar including
        /// the stack trace. 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>
        /// <param name="level">Default is "error". "critical" and "warning" may also make sense to use.</param>
        /// <param name="modelAction"></param>
        void SendException(Exception ex, string title = null, string level = "error", Action<DataModel> modelAction = null, object userParam = null);

        /// <summary>
        /// Sends a text notice using the "critical" level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="customData"></param>
        /// <param name="modelAction"></param>
        void SendCriticalMessage(string message, IDictionary<string, object> customData = null, Action<DataModel> modelAction = null, object userParam = null);

        /// <summary>
        /// Sents a text notice using the "error" level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="customData"></param>
        /// <param name="modelAction"></param>
        void SendErrorMessage(string message, IDictionary<string, object> customData = null, Action<DataModel> modelAction = null, object userParam = null);

        /// <summary>
        /// Sends a text notice using the "warning" level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="customData"></param>
        /// <param name="modelAction"></param>
        void SendWarningMessage(string message, IDictionary<string, object> customData = null, Action<DataModel> modelAction = null, object userParam = null);

        /// <summary>
        /// Sends a text notice using the "info" level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="customData"></param>
        /// <param name="modelAction"></param>
        void SendInfoMessage(string message, IDictionary<string, object> customData = null, Action<DataModel> modelAction = null, object userParam = null);

        /// <summary>
        /// Sends a text notice using the "debug" level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="customData"></param>
        /// <param name="modelAction"></param>
        void SendDebugMessage(string message, IDictionary<string, object> customData = null, Action<DataModel> modelAction = null, object userParam = null);

        /// <summary>
        /// Sents a text notice using the given level of severity
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="customData"></param>
        /// <param name="modelAction"></param>
        void SendMessage(string message, string level, IDictionary<string, object> customData = null, Action<DataModel> modelAction = null, object userParam = null);

        void Send(DataModel data, object userParam);

        /// <summary>
        /// Serialize the given object for transmission
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string Serialize(object data);
    }
}