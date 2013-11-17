using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartGPS.Business;
using smartGPS.Custom;

namespace smartGPS.Areas.Dashboard.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/Dashboard/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPut]
        public ActionResult UpdateUserLocation(String latitude, String longitude)
        {
            UserAdministration.updateUserLocation(Double.Parse(latitude, CultureInfo.InvariantCulture), Double.Parse(longitude, CultureInfo.InvariantCulture), User.Identity.Name);
            return Content("User location updated!");
        }
    }
}
