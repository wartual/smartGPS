using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartGPS.BusinessLogic;
using smartGPS.Custom;

namespace smartGPS.Areas.Dashboard.Controllers
{
    [smartGPSAuthorize]
    public class UserManagementController : Controller
    {
        //
        // GET: /Dashboard/UserMangement/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignOut()
        {
            UserAdministration.signOut();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
