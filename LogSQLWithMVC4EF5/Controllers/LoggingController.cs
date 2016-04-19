using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogSQLWithMVC4EF5.Controllers
{
    public class LoggingController : Controller
    {
        public virtual ActionResult SqlLogDisplay() {
            return PartialView();
        }

        public virtual ActionResult GetDebugLogInformation() {
            var userName = User.Identity.Name;

            return Json(new { userName }, JsonRequestBehavior.AllowGet);
        }

    }
}
