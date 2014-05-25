using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using smartGPS.Areas.Dashboard.Models;
using smartGPS.Business;
using smartGPS.Persistance;

namespace smartGPS.Areas.Dashboard.Controllers
{
    public class HistoryController : BaseDashboardController
    {

        public ActionResult Index()
        {
            List<Travel> travels = TravelManager.getAllByUserId(User.Identity.Name);
            List<TravelModel> viewModel = new List<TravelModel>();

            if (travels != null)
            {
                foreach (Travel model in travels)
                {
                    viewModel.Add(mapDBTravelToTravelModel(model));
                }
            }
            return View(viewModel);
        }

        /*
        public ActionResult Index()
        {
            return View();
        }*/

        #region Utilities

        private TravelModel mapDBTravelToTravelModel(Travel travel)
        {
            TravelModel model = new TravelModel();
            model.CurrentLatitude = travel.CurrentLatitude;
            model.CurrentLongitude = travel.CurrentLongitude;
            model.DateCreated = travel.DateCreated;
            model.DateUpdated = travel.DateUpdated;
            model.DepartureAddress = travel.DepartudeAddress;
            model.DepartureLatitude = travel.DepartureLatitude;
            model.DepartureLongitude = travel.DepartudeLongitude;
            model.DestinationAddress = travel.DestinationAddress;
            model.DestinationLatitude = travel.DestinationLatitude;
            model.DestinationLongitude = travel.DestinationLongitude;
            model.Directions = travel.Directions;
            model.Distance = travel.Distance;

            IEnumerable<TravelStatusCategory> travelCategories = TravelManager.getAllTravelStatus();
            Boolean statusSet = false;
            foreach (TravelStatusCategory category in travelCategories)
            {
                if (travel.StatusId.Equals(category.Id))
                {
                    model.Status = category.Type;
                    statusSet = true;
                }
            }

            if(!statusSet)
                model.Status = "Unknown";

            return model;
        }

        #endregion 
    }
}
