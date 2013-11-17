using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoCoding;
using Newtonsoft.Json;
using smartGPS.Areas.Dashboard.Models;
using smartGPS.Business;
using smartGPS.Business.ExternalServices;
using smartGPS.Custom;
using smartGPS.Persistance;

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
            SetupTravelModel model = new SetupTravelModel();
            ViewBag.TypesOfNavigation = setupTypeOfNavigation();
            return PartialView("_SetupNavigation", model);
        }

        private List<SelectListItem> setupTypeOfNavigation()
        {
            List<SelectListItem> typesOfNavigation = new List<SelectListItem>();

            typesOfNavigation.Add(new SelectListItem { Text = "Select navigation type", Value = "0" , Selected = true});
            typesOfNavigation.Add(new SelectListItem { Text = "Address", Value = "1" });
            typesOfNavigation.Add(new SelectListItem { Text = "Gps coordinates", Value = "2" });
            
            return typesOfNavigation;
        }

        public JsonResult GetAddressByGPSCoordinates(String latitude, String longitude)
        {
            IEnumerable<Address> address = ExternalUtilities.getAddressesFromGpsCoordinates(Double.Parse(latitude, CultureInfo.InvariantCulture), Double.Parse(longitude, CultureInfo.InvariantCulture));
            List<SmartDestinationJson> list = new List<SmartDestinationJson>();
            foreach (Address model in address)
            {
                SmartDestinationJson json = new SmartDestinationJson();
                json.Address = model.FormattedAddress;
                list.Add(json);
            }

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
