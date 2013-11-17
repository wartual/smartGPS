using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartGPS.Custom;

namespace smartGPS.Areas.Dashboard.Controllers
{
    [smartGPSAuthorize]
    public class TravelController : Controller
    {
        //
        // GET: /Dashboard/Travel/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewTravel()
        {

            return PartialView("");
        }
    }
}
