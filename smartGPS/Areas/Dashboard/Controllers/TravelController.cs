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
        
        private static IEnumerable<Address> locations;
        public static String DEPARTURE_LATITUDE = "departure_latitude";
        public static String DEPARTURE_LONGITUDE = "departure_longitude";
        public static String DESTINATION_LATITUDE = "destination_latitude";
        public static String DESTINATION_LONGITUDE = "destination_longitude";

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult NewTravel()
        {
            SetupTravelModel model = new SetupTravelModel();
            UserHelper helper = UserAdministration.getUserHelper(User.Identity.Name);

            if (helper.LastLocationLatitude != null && helper.LastLocationLongitude != null)
            {
                model.StartLatitude = helper.LastLocationLatitude.ToString();
                model.StartLongitude = helper.LastLocationLongitude.ToString();
                locations = GoogleManagement.getLocationsFromGpsCoordinates(helper.LastLocationLatitude.Value, helper.LastLocationLongitude.Value);
                model.StartAddress = locations.FirstOrDefault().FormattedAddress;
            }

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
                if (viewModel.DestinationType.Equals("2") && viewModel.Destination == null && (viewModel.Latitude == null || viewModel.Longitude == null))
                {
                    return RedirectToAction("TravelError");
                }
                else if (viewModel.DestinationType.Equals("1") && viewModel.DestinationDropdown == null)
                {
                    return RedirectToAction("TravelError");
                }
                else if (viewModel.DestinationType.Equals(null))
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
                ViewBag.TypesOfNavigation = new SelectList(setupTypeOfNavigation(), "Value", "Text");
                ViewBag.Destinations = new SelectList(new List<SelectListItem>());
                return View(viewModel);
            }
        }


        [HttpGet]
        public ActionResult PreviewTravel(SetupTravelModel model)
        {
            try
            {
                Dictionary<string, double> endpoints = obtainDepartureAndDestinationLocation(model);
                DirectionsModel viewModel = new DirectionsModel();
                GoogleMapsDirectionsResponse googleDirections = null;


                googleDirections = GoogleManagement.getDataFromGoogleMapsApis(User.Identity.Name, endpoints[DEPARTURE_LATITUDE], endpoints[DEPARTURE_LONGITUDE],
                                     endpoints[DESTINATION_LATITUDE], endpoints[DESTINATION_LONGITUDE], DirectionsMode.MODE_DRIVING);
              

                viewModel = mapGoogleDirectionsToDirectionsModel(viewModel, googleDirections, null);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return View("NewTravle");
            }

        }

        #region Utilities

        private Dictionary<string, double> obtainDepartureAndDestinationLocation(SetupTravelModel model)
        {
            Dictionary<string, double> endpoints = new Dictionary<string, double>();
            Geometry geometry = new Geometry();

            if (model.DestinationType.Equals("1"))
            {
                geometry = getGeometryFromAddress(model.Address, model.DestinationDropdown.Value);
                endpoints.Add(DESTINATION_LATITUDE, geometry.Locations.Latitude);
                endpoints.Add(DESTINATION_LONGITUDE, geometry.Locations.Longitude);
            }
            else if(model.DestinationType.Equals("2"))
            {
                endpoints.Add(DESTINATION_LATITUDE, Utilities.parseDouble(model.Latitude).Value);
                endpoints.Add(DESTINATION_LONGITUDE, Utilities.parseDouble(model.Longitude).Value);
            }

            if (model.DepartureType.Equals("1"))
            {
                geometry = getGeometryFromAddress(model.StartAddress, model.DepartureDropdown.Value);
                endpoints.Add(DEPARTURE_LATITUDE, geometry.Locations.Latitude);
                endpoints.Add(DEPARTURE_LONGITUDE, geometry.Locations.Longitude);
            }
            else if(model.DepartureType.Equals("2"))
            {
                endpoints.Add(DEPARTURE_LATITUDE, Utilities.parseDouble(model.StartLatitude).Value);
                endpoints.Add(DEPARTURE_LONGITUDE, Utilities.parseDouble(model.StartLongitude).Value);
            }

            return endpoints;
        }

        private Geometry getGeometryFromAddress(String address, int index)
        {
            GoogleGeoCodeResponse googleGeocoder;
            locations = GoogleManagement.getLocationsFromAddress(address);
            String formattedAddress = locations.ElementAt(index).FormattedAddress;
            googleGeocoder = GoogleManagement.getDataFromGoogleApis(address);
            return googleGeocoder.Results.ElementAt(0).Geometry;
        }

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
            directions.PolyLines = googleDirections.Routes[0].OverviewPolyline.Points;
            directions.NortheastBound = googleDirections.Routes[0].Bounds.Northeast;
            directions.SouthwestBound = googleDirections.Routes[0].Bounds.Southwest;
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

        #endregion


        #region ExternalCalls

        public JsonResult GetAddressByGPSCoordinates(String latitude, String longitude)
        {
            locations = GoogleManagement.getLocationsFromGpsCoordinates(Double.Parse(latitude, CultureInfo.InvariantCulture), Double.Parse(longitude, CultureInfo.InvariantCulture));
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
            locations = GoogleManagement.getLocationsFromAddress(address);
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

        public JsonResult GetDirections(String startLatitude, String startLongitude, String endLatitude, String endLongitude, String mode)
        {
            GoogleMapsDirectionsResponse googleDirections = GoogleManagement.getDataFromGoogleMapsApis(User.Identity.Name,
                                                                        Double.Parse(startLatitude.Replace(",", "."), CultureInfo.InvariantCulture),
                                                                        Double.Parse(startLongitude.Replace(",", "."), CultureInfo.InvariantCulture),
                                                                        Double.Parse(endLatitude.Replace(",", "."), CultureInfo.InvariantCulture),
                                                                        Double.Parse(endLongitude.Replace(",", "."), CultureInfo.InvariantCulture), mode);

            return this.Json(mapGoogleDirectionsToDirectionsModel(new DirectionsModel(), googleDirections, mode), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPlaces(String latitude, String longitude)
        {
            double endLatitude = Double.Parse(latitude.Replace(",", "."), CultureInfo.InvariantCulture);
            double endLongitude = Double.Parse(longitude.Replace(",", "."), CultureInfo.InvariantCulture);

            GooglePlacesResponse googlePlaces = GoogleManagement.getDataFromGooglePlaces(User.Identity.Name,
                                                                        endLatitude, endLongitude);

            return this.Json(googlePlaces, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
