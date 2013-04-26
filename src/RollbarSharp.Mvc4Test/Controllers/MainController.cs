using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace RollbarSharp.Mvc4Test.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Home()
        {
            FormsAuthentication.SetAuthCookie("mroach", true);

            var vars = Request.ServerVariables.AllKeys.Select(k => k + " => " + Request.ServerVariables[k]);

            ViewBag.Vars = string.Join("\n", vars);

            return View();
        }

        public ActionResult Error()
        {
            var ex = new Exception("An error has occurred upon request");
            ex.Data["arbitrary_data"] = DateTime.UtcNow;
            throw ex;
        }
    }
}