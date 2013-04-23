using System;
using System.Web;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class ServerModelBuilder
    {
        /// <summary>
        /// Creates a <see cref="ServerModel"/> using data from the given
        /// <see cref="HttpRequest"/>. Finds: HTTP Host, Server Name, Application Physical Path
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ServerModel CreateFromHttpRequest(HttpRequest request)
        {
            var host = request.ServerVariables.Get("HTTP_HOST");

            if (string.IsNullOrEmpty(host))
                host = request.ServerVariables.Get("SERVER_NAME");

            var root = request.ServerVariables.Get("APPL_PHYSICAL_PATH");

            if (string.IsNullOrEmpty(root))
                root = HttpRuntime.AppDomainAppPath ?? Environment.CurrentDirectory;

            return new ServerModel { Host = host, Root = root };
        }

        /// <summary>
        /// Create a <see cref="ServerModel"/> using the current <see cref="HttpRequest"/>
        /// </summary>
        /// <returns></returns>
        public static ServerModel CreateFromCurrentRequest()
        {
            var cx = HttpContext.Current;

            return cx == null ? new ServerModel() : CreateFromHttpRequest(cx.Request);
        }
    }
}
