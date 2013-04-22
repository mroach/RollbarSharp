using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class FrameModelBuilder
    {
        /// <summary>
        /// Try to include file names in the stack trace rather than just method names with internal offsets
        /// </summary>
        public static bool UseFileNames = true;

        /// <summary>
        /// Recursively add internal exceptions to the stack trace
        /// </summary>
        public static bool IncludeInternalExceptions = true;

        /// <summary>
        /// Converts the Exception's stack trace to simpler models.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        /// <remarks>If we want to get fancier, we can rip off ideas from <see cref="StackTrace.ToString()"/></remarks>
        public static FrameModel[] CreateFramesFromException(Exception exception)
        {
            var lines = new List<FrameModel>();
            var stackFrames = new StackTrace(exception, UseFileNames).GetFrames();

            if (stackFrames == null || stackFrames.Length == 0)
                return lines.ToArray();

            foreach (var frame in stackFrames)
            {
                var method = frame.GetMethod();
                var lineNumber = frame.GetFileLineNumber();
                var fileName = frame.GetFileName();
                var methodParams = method.GetParameters();

                if (lineNumber == 0)
                    lineNumber = frame.GetILOffset();

                if (lineNumber == -1)
                    lineNumber = frame.GetNativeOffset();

                if (lineNumber < 0)
                    lineNumber = 0;

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = method.ReflectedType != null
                                   ? method.ReflectedType.FullName
                                   : "(unknown)";
                }

                var methodName = method.Name;

                if (methodParams.Length > 0)
                {
                    var paramDesc = string.Join(", ", methodParams.Select(p => p.ParameterType + " " + p.Name));
                    methodName = methodName + "(" + paramDesc + ")";
                }

                lines.Add(new FrameModel(fileName, lineNumber, methodName));
            }

            if (IncludeInternalExceptions && exception.InnerException != null)
            {
                lines.Add(new FrameModel("-- INNER EXCEPTION " + exception.InnerException.GetType() + " --"));
                lines.AddRange(CreateFramesFromException(exception.InnerException));
            }


            return lines.ToArray();
        }
    }
}
