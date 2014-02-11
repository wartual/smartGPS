using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartGPS.Business;
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
            CBA cba = new CBA(User.Identity.Name);
            KNNAlgorithm knn = new KNNAlgorithm(User.Identity.Name, 10);
            DecisionTreesAlgorithm decisionTrees = new DecisionTreesAlgorithm(User.Identity.Name, 2, dummyModel());
            SVMAlgorithm svm = new SVMAlgorithm(User.Identity.Name);
            //decisionTrees.runAlgorithm();
            //cba.performClassification();
            //knn.testData();
            svm.runAlgorithm();
            
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