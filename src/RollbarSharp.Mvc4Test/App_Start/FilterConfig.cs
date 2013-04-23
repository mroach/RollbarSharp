using System.IO;
using System.Web.Mvc;

namespace RollbarSharp.Mvc4Test
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RollbarExceptionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }

    public class RollbarExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            var client = new RollbarClient();
            client.RequestCompleted += client_RequestCompleted;
            client.SendException(filterContext.Exception);
        }

        void client_RequestCompleted(object source, RequestCompletedEventArgs args)
        {
            File.WriteAllText(@"c:\users\mroach\stuff\debug.txt", args.Result.ToString());
        }
    }
}