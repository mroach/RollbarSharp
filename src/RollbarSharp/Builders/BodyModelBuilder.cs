using System;
using System.Collections.Generic;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    /// <summary>
    /// Builder for the 'body' of the request.
    /// This will be either an exception with details
    /// or a plain text message with optional fields
    /// </summary>
    public static class BodyModelBuilder
    {
        public static ExceptionBodyModel CreateExceptionBody(Exception exception)
        {
            var traces = TraceChainModelBuilder.CreateFromException(exception);
            return new ExceptionBodyModel(traces);
        }

        public static MessageBodyModel CreateMessageBody(string message, IDictionary<string, object> customData)
        {
            return new MessageBodyModel(message, customData);
        }
    }
}
