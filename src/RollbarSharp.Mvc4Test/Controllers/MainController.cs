using System;
using System.Web.Mvc;
using System.Web.Security;

namespace RollbarSharp.Mvc4Test.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Home()
        {
            FormsAuthentication.SetAuthCookie("mroach", true);
            return View();
        }

        public ActionResult Error()
        {
            throw new Exception("An error has occurred upon request");
        }
    }
}