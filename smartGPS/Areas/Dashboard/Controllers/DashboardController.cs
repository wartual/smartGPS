using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartGPS.Areas.Dashboard.Models;
using smartGPS.Business;
using smartGPS.Business.AStar;
using smartGPS.Business.CBA;
using smartGPS.Business.DecisionTrees;
using smartGPS.Business.ExternalServices;
using smartGPS.Business.KMeansAlgorithm;
using smartGPS.Business.KNN;
using smartGPS.Business.Models.Foursquare;
using smartGPS.Business.Models.GoogleModels;
using smartGPS.Business.SVM;
using smartGPS.Custom;
using smartGPS.Persistance;

namespace smartGPS.Areas.Dashboard.Controllers
{
    public class DashboardController : BaseDashboardController
    {
        //
        // GET: /Dashboard/Dashboard/

        public ActionResult Index()
        {
            Profile profile = UserAdministration.getProfileByUserId(User.Identity.Name);
            ProfileModel viewModel = smartGPS.Areas.Administration.Models.Mapping.usersToProfileModel(profile);

            return View(viewModel);
        }

        [HttpPut]
        public ActionResult UpdateUserLocation(String latitude, String longitude)
        {
            UserAdministration.updateUserLocation(Double.Parse(latitude, CultureInfo.InvariantCulture), Double.Parse(longitude, CultureInfo.InvariantCulture), User.Identity.Name);
            return Content("User location updated!");
        }

        private FacebookProccesedEntries dummyModel()
        {
            FacebookProccesedEntries model = new FacebookProccesedEntries();
            model.UserId = User.Identity.Name;
            model.UserName = "Kristina Krznarić";
            model.LikesBooks = "Unknown";
            model.LikesMovies = "True";
            model.LikesMusic = "True";
            model.LikesSports = "True";
            model.LikesTravelling = "True";
            model.Sportsman = "False";
            return model;
        }
    }
}