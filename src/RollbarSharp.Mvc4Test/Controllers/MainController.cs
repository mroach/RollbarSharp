using System;
using System.Collections.Generic;
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

            Session["UserId"] = 102;
            Session["CartAmount"] = 123.45m;
            Session["Username"] = "mroach";
            Session["CartItems"] = new[] {233, 479073, 2323, 9923};
            Session["Userdata"] = new HashSet<string>();
            Session["Password"] = "y u store password?";
            Session["Secret"] = "A23A854F3DD64CC9BEC6D498A2C097D8";
            Session["secret"] = "another secret :o";

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