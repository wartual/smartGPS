using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartGPS.Business;
using smartGPS.Business.CBA;
using smartGPS.Custom;

namespace smartGPS.Areas.Dashboard.Controllers
{
    public class DashboardController : BaseDashboardController
    {
        //
        // GET: /Dashboard/Dashboard/

        public ActionResult Index()
        { 
            CBA_RG ruleGenerator = new CBA_RG(0.1, 0.3, User.Identity.Name);
            ruleGenerator.getRulesSet();
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
