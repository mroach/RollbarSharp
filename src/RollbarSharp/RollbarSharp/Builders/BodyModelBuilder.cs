using System;
using System.Collections.Generic;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class BodyModelBuilder
    {
        public static ExceptionBodyModel CreateExceptionBody(Exception exception)
        {
            var trace = TraceModelBuilder.CreateFromException(exception);
            return new ExceptionBodyModel(trace);
        }

        public static MessageBodyModel CreateMessageBody(string message, IDictionary<string, string> customData)
        {
            return new MessageBodyModel(message, customData);
        }
    }
}
