using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartGPS.Business;
using smartGPS.Business.AStar;
using smartGPS.Business.CBA;
using smartGPS.Business.DecisionTrees;
using smartGPS.Business.KMeansAlgorithm;
using smartGPS.Business.KNN;
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
            PathSearch pathSearch = new PathSearch();
           // pathSearch.search(46.056450999999996, 14.50807, 46.0744955, 14.48583009999993, 11);
            pathSearch.search(46.05148532366063, 14.505992531776428, 46.05230064782611, 14.503664374351501, 11);
            return View();
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