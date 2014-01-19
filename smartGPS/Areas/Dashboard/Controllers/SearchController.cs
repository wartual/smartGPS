using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using smartGPS.Areas.Dashboard.Models;
using smartGPS.Business;
using smartGPS.Business.ExternalServices;
using smartGPS.Business.Models.GoogleModels;

namespace smartGPS.Areas.Dashboard.Controllers
{
    public class SearchController : BaseDashboardController
    {

        public ActionResult SearchPlaces()
        {
            SearchModel viewModel = new SearchModel();
            ViewBag.TypesOfNavigation = new SelectList(setupTypeOfNavigation(), "Value", "Text");
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SearchPlaces(SearchModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.SearchType.Equals("2") && (viewModel.Latitude == null || viewModel.Longitude == null))
                {
                    return RedirectToAction("TravelError");
                }
                else if (viewModel.SearchType.Equals("1") && viewModel.Place == null)
                {
                    return RedirectToAction("TravelError");
                }
                else if (viewModel.SearchType.Equals(null))
                {
                    return RedirectToAction("TravelError");
                }
                else
                {
                    return RedirectToAction("Places", viewModel);
                }
            }
            else
            {
                ViewBag.TypesOfNavigation = new SelectList(setupTypeOfNavigation(), "Value", "Text");
                ViewBag.Destinations = new SelectList(new List<SelectListItem>());
                return View(viewModel);
            }
        }

        public ActionResult Places(SearchModel viewModel)
        {
            GooglePlacesResponse googlePlaces;
            if(viewModel.SearchType.Equals("1"))
                googlePlaces = GoogleManagement.getDataFromGooglePlacesByText(viewModel.Place);
            else{
                double queryLatitude = Utilities.parseDouble(viewModel.Latitude).Value;
                double queryLongitude = Utilities.parseDouble(viewModel.Longitude).Value;
                googlePlaces = GoogleManagement.getDataFromGooglePlaces(queryLatitude, queryLongitude);

            }

            return View(googlePlaces.Results);
        }
    }
}
