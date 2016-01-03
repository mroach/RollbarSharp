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

                // when the line number is zero, you can try using the IL offset
                if (lineNumber == 0)
                    lineNumber = frame.GetILOffset();

                if (lineNumber == -1)
                    lineNumber = frame.GetNativeOffset();

                // line numbers less than 0 are not accepted
                if (lineNumber < 0)
                    lineNumber = 0;

				string methodName;

				//At least on Mono 4.2.1: on reflection call frame.GetMethod() can be null
				if (method != null) {
					// file names aren't always available, so use the type name instead, if possible
					if (string.IsNullOrEmpty(fileName)) {
						fileName = method.ReflectedType != null
	                                   ? method.ReflectedType.FullName
	                                   : "(unknown)";
					}
									
					methodName = method.Name;

					// add method parameters to the method name. helpful for resolving overloads.
					var methodParams = method.GetParameters();
					if (methodParams.Length > 0) {
						var paramDesc = string.Join(", ", methodParams.Select(p => p.ParameterType + " " + p.Name));
						methodName = methodName + "(" + paramDesc + ")";
					}
				} else {
					methodName = "(unknown)";
				}

                lines.Add(new FrameModel(fileName, lineNumber, methodName));
            }

            return lines.ToArray();
        }
    }
}
