using System;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class TraceModelBuilder
    {
        /// <summary>
        /// Converts an <see cref="Exception"/> to <see cref="ExceptionModel"/>
        /// and <see cref="FrameModel"/>[] and wraps them in a <see cref="TraceModel"/>
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static TraceModel CreateFromException(Exception exception)
        {
            var exceptionModel = ExceptionModelBuilder.CreateFromException(exception);
            var frames = FrameModelBuilder.CreateFramesFromException(exception);

            return new TraceModel(exceptionModel, frames);
        }
    }
}
