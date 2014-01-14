using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace smartGPS.Areas.Dashboard.Controllers
{
    public class BaseDashboardController : Controller
    {
        public static List<SelectListItem> setupTypeOfNavigation()
        {
            List<SelectListItem> typesOfNavigation = new List<SelectListItem>();

            typesOfNavigation.Add(new SelectListItem { Text = "Address", Value = "1" });
            typesOfNavigation.Add(new SelectListItem { Text = "Gps coordinates", Value = "2" });

            return typesOfNavigation;
        }
    }
}
