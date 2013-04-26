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

            (new RollbarClient()).SendException(filterContext.Exception);
        }
    }
}