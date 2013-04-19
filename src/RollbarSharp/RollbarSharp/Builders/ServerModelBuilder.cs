using System;
using System.Web;
using RollbarSharp.Serialization;

namespace RollbarSharp.Builders
{
    public static class ServerModelBuilder
    {
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

        public static ServerModel CreateFromCurrentRequest()
        {
            var cx = HttpContext.Current;

            return cx == null ? new ServerModel() : CreateFromHttpRequest(cx.Request);
        }
    }
}
