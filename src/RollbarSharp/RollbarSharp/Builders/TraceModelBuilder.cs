using System;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class TraceModelBuilder
    {
        public static TraceModel CreateFromException(Exception exception)
        {
            var exceptionModel = ExceptionModelBuilder.CreateFromException(exception);
            var frames = FrameModelBuilder.CreateFramesFromException(exception);

            return new TraceModel(exceptionModel, frames);
        }
    }
}
