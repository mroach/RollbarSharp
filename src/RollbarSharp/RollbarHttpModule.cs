using System;
using System.Web;

namespace RollbarSharp
{
    public class RollbarHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.Error += SendError;
        }

        public void Dispose()
        {
        }

        private static void SendError(object sender, EventArgs e)
        {
            var application = (HttpApplication) sender;
            var ex = application.Server.GetLastError();

            if (ex is HttpUnhandledException)
                ex = ex.InnerException;

            new RollbarClient().SendException(ex);
        }

    }
}