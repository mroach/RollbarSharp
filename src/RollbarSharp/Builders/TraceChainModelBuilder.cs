using System;
using System.Collections.Generic;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class TraceChainModelBuilder
    {
        /// <summary>
        /// Converts an <see cref="Exception"/> ands his InnerExceptions to
        /// <see cref="TraceModel"/>'s.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static IEnumerable<TraceModel> CreateFromException(Exception exception)
        {
            var traces = new List<TraceModel>();
            var innerEx = exception;

            while (innerEx != null)
            {
                var exceptionModel = ExceptionModelBuilder.CreateFromException(innerEx);
                var frames = FrameModelBuilder.CreateFramesFromException(innerEx);

                traces.Insert(0, new TraceModel(exceptionModel, frames));

                innerEx = innerEx.InnerException;
            }

            return traces;
        }
    }
}