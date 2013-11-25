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
using smartGPS.Business.Models.GoogleModels;
using smartGPS.Custom;
using smartGPS.Persistance;

namespace smartGPS.Areas.Dashboard.Controllers
{
    [smartGPSAuthorize]
    public class TravelController : Controller
    {
        //
        // GET: /Dashboard/Travel/

        private static IEnumerable<Address> locations;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewTravel()
        {
            SetupTravelModel model = new SetupTravelModel();
            ViewBag.TypesOfNavigation = new SelectList(setupTypeOfNavigation(), "Value", "Text");
            ViewBag.Destinations = new SelectList(new List<SelectListItem>());
            return PartialView("_SetupNavigation", model);
        }

        public ActionResult TravelError()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SetupTravel(SetupTravelModel viewModel)
        {
            // check if model is valid
            if (ModelState.IsValid)
            {
                if (viewModel.NavigationType.Equals("2") && viewModel.Destination == null && (viewModel.Latitude == null || viewModel.Longitude == null))
                {
                    return RedirectToAction("TravelError");
                }
                else if (viewModel.NavigationType.Equals("1") && viewModel.DestinationDropdown == null)
                {
                      return RedirectToAction("TravelError");
                }
                else if (viewModel.NavigationType.Equals(null))
                {
                    return RedirectToAction("TravelError");
                }
                else
                {
                    return RedirectToAction("PreviewTravel", viewModel);
                }
            }
            else
            {
                return RedirectToAction("TravelError");
            }
        }


        [HttpGet]
        public ActionResult PreviewTravel(SetupTravelModel model)
        {
            DirectionsModel viewModel = new DirectionsModel();
            GoogleMapsDirectionsResponse googleDirections = null;
            GoogleGeoCodeResponse googleGeocoder;

            if(model.NavigationType.Equals("1")){
                locations = ExternalUtilities.getLocationsFromAddress(model.Address);
                int index = model.DestinationDropdown.Value;
                String address = locations.ElementAt(index).FormattedAddress;
                googleGeocoder = ExternalUtilities.getDataFromGoogleApis(address);
                Geometry geometry = googleGeocoder.Results.ElementAt(0).Geometry;
                googleDirections = ExternalUtilities.getDataFromGoogleMapsApis(User.Identity.Name, geometry.Locations.Latitude, geometry.Locations.Longitude, DirectionsMode.MODE_DRIVING);
            }
            else if(model.NavigationType.Equals("2")){
                googleDirections = ExternalUtilities.getDataFromGoogleMapsApis(User.Identity.Name, model.Latitude.Value, model.Longitude.Value
                            , DirectionsMode.MODE_DRIVING);
            }

            viewModel = mapGoogleDirectionsToDirectionsModel(viewModel, googleDirections, null);
            return View(viewModel);
        }

        #region Utilities

        private DirectionsModel mapGoogleDirectionsToDirectionsModel(DirectionsModel directions, GoogleMapsDirectionsResponse googleDirections, String mode)
        {
            directions.StartAddress = googleDirections.Routes[0].Legs[0].StartAddress;
            directions.EndAddress = googleDirections.Routes[0].Legs[0].EndAddress;
            directions.StartLocation = googleDirections.Routes[0].Legs[0].StartLocation;
            directions.EndLocation = googleDirections.Routes[0].Legs[0].EndLocation;
            directions.DurationText = googleDirections.Routes[0].Legs[0].Duration.Text;
            directions.DistanceText = googleDirections.Routes[0].Legs[0].Distance.Text;
            directions.DurationValue = googleDirections.Routes[0].Legs[0].Duration.Value;
            directions.DistanceValue = googleDirections.Routes[0].Legs[0].Distance.Value;
            directions.Modes = setupModes();
            if (mode == null)
            {
                directions.Mode = 1;
            }
            else
            {
                directions.Mode = Int16.Parse(directions.Modes.Where(item => item.Text.ToLower().Equals(mode.ToLower())).FirstOrDefault().Value);
            }
           
            return directions;
        
        }

        private List<SelectListItem> setupTypeOfNavigation()
        {
            List<SelectListItem> typesOfNavigation = new List<SelectListItem>();

            typesOfNavigation.Add(new SelectListItem { Text = "Address", Value = "1" });
            typesOfNavigation.Add(new SelectListItem { Text = "Gps coordinates", Value = "2" });

            return typesOfNavigation;
        }

        private List<SelectListItem> setupModes()
        {
            List<SelectListItem> typesOfNavigation = new List<SelectListItem>();

            typesOfNavigation.Add(new SelectListItem { Text = "Driving", Value = "1" });
            typesOfNavigation.Add(new SelectListItem { Text = "Walking", Value = "2" });

            return typesOfNavigation;
        }

        public JsonResult GetAddressByGPSCoordinates(String latitude, String longitude)
        {
            locations = ExternalUtilities.getLocationsFromGpsCoordinates(Double.Parse(latitude, CultureInfo.InvariantCulture), Double.Parse(longitude, CultureInfo.InvariantCulture));
            List<SmartDestinationJson> list = new List<SmartDestinationJson>();
            foreach (Address model in locations)
            {
                SmartDestinationJson json = new SmartDestinationJson();
                json.Address = model.FormattedAddress;
                list.Add(json);
            }

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDestinationsByAddress(String address)
        {
            locations = ExternalUtilities.getLocationsFromAddress(address);
            List<SmartDestinationJson> list = new List<SmartDestinationJson>();
            int i = 0;
            foreach (Address model in locations)
            {
                SmartDestinationJson json = new SmartDestinationJson();
                json.Address = model.FormattedAddress;
                json.Value = i;
                list.Add(json);
                i++;
            }

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDirections(String latitude, String longitude, String mode)
        {
            double endLatitude = Double.Parse(latitude.Replace(",","."), CultureInfo.InvariantCulture);
            double endLongitude= Double.Parse(longitude.Replace(",","."), CultureInfo.InvariantCulture);

            GoogleMapsDirectionsResponse googleDirections = ExternalUtilities.getDataFromGoogleMapsApis(User.Identity.Name,
                                                                        endLatitude, endLongitude, mode);

            return this.Json(mapGoogleDirectionsToDirectionsModel(new DirectionsModel(), googleDirections, mode), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
