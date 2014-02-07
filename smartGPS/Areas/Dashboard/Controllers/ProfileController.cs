using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartGPS.Areas.Administration.Models;
using smartGPS.Areas.Dashboard.Models;
using smartGPS.Business;
using smartGPS.Business.ExternalServices;
using smartGPS.Business.Models;
using smartGPS.Business.Models.Facebook;
using smartGPS.Business.Models.Foursquare;
using smartGPS.Custom;
using smartGPS.Persistance;

namespace smartGPS.Areas.Dashboard.Controllers
{
    [smartGPSAuthorize]
    public class ProfileController : BaseDashboardController
    {
        //
        // GET: /Dashboard/Profile/

        public ActionResult Index()
        {
            Profile profile = UserAdministration.getProfileByUserId(User.Identity.Name);
            ProfileModel viewModel = smartGPS.Areas.Administration.Models.Mapping.usersToProfileModel(profile);
            PrometInfoManagement.getInfo();
            OpenWeatherManagement.getWeatherByGPS(44, 16); 
            return View(viewModel);
        }

        public ActionResult ShowFacebookStatistics()
        {
            Profile profile = UserAdministration.getProfileByUserId(User.Identity.Name);
            ProfileModel viewModel = smartGPS.Areas.Administration.Models.Mapping.usersToProfileModel(profile);
            FacebookStatisticsModel model = TempData["statistics"] as FacebookStatisticsModel;
            viewModel.FacebookStatistics = model;

            return View("Index",viewModel);
        }

        public ActionResult ShowFoursquareStatistics()
        {
            FoursquareProfile model = TempData["profile"] as FoursquareProfile;
            FoursquareStatistics statistics;

            if (model == null)
            {
                statistics = FourqsquareManagement.importVenusHistory(User.Identity.Name);
            }
            else
            {
                statistics = FourqsquareManagement.getStatistics(model);
            }

            Profile profile = UserAdministration.getProfileByUserId(User.Identity.Name);
            ProfileModel viewModel = smartGPS.Areas.Administration.Models.Mapping.usersToProfileModel(profile);
            viewModel.FoursquareStatistics = prepareFoursquareStatisticsModel(statistics);
            return View("Index", viewModel);
        }

        [HttpPost]
        public ActionResult Profile(ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                Boolean status = UserAdministration.updateProfile(User.Identity.Name, model.Username, model.Name, model.Surname, null,
                                                                   model.Gender, model.Email, model.Phone, model.Address, model.PostalOffice, model.Country, model.DateOfBirth);
                if (status)
                {
                    ViewBag.Message = "User profile has been saved!";
                    return View("Index", model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error has occured while saving to database");
                    return View("Index", model);
                }

            }
            else
            {
                return View("Index", model);
            }
        }

        public ActionResult FacebookStatistics(String token)
        {
            FacebookProfile fbProfile = FacebookManagement.getFacebookProfileData(User.Identity.Name);
            FacebookStatistics statistics;

            if (fbProfile == null)
            {
                FacebookManagement.importFacebookData(token, User.Identity.Name);
                FacebookManagement.importCheckins(token, User.Identity.Name);
                FacebookManagement.importLikes(token, User.Identity.Name);
            }
            else
            {
                if(fbProfile.JsonPersonalDataAndFriends == null)
                {
                    FacebookManagement.importFacebookData(token, User.Identity.Name);
                }
                else if(fbProfile.JsonUserAndFriendsCheckins == null)
                {
                    FacebookManagement.importCheckins(token, User.Identity.Name);
                }
                else if(fbProfile.JsonUserAndFriendsLikes == null)
                {
                    FacebookManagement.importLikes(token, User.Identity.Name);
                }
            }
            
            fbProfile = FacebookManagement.getFacebookProfileData(User.Identity.Name);
            statistics = FacebookManagement.getStatistics(fbProfile);
         
            if (statistics != null)
            {
                FacebookStatisticsModel model = prepareFacebookStatisticsModel(statistics);
                TempData["statistics"] = model;
                return RedirectToAction("ShowFacebookStatistics");
            }
            else
                return RedirectToAction("ExternalLoginFailure");
        }

        public ActionResult FacebookProfile(FacebookStatistics statistics)
        {

            return View();
        }

        #region Utils

        private FacebookStatisticsModel prepareFacebookStatisticsModel(FacebookStatistics statistics)
        {
            FacebookStatisticsModel model = new FacebookStatisticsModel();

            model.CountFriendsAnalzyed = statistics.CountFriendsAnalzyed;
            model.CountFriendsLikes = statistics.CountFriendsLikes;
            model.CountUserLikes = statistics.CountUserLikes;
            model.FriendsLikesCategoriesFrequency = statistics.FriendsLikesCategoriesFrequency;
            model.FriendsLikesFrequency = statistics.FriendsLikesFrequency;
            model.Name = statistics.Name;
            model.FriendsCheckinsFrequency = statistics.FriendsCheckinsFrequency;
            model.UserCheckinsFrequency = statistics.UserCheckinsFrequency;
            model.SimilarFriends = statistics.similarFriends;
            model.UserLikesCategoriesFrequency = statistics.UserLikesCategoriesFrequency;
            model.LikesBooks = Utilities.mapStatusEnumToWord(statistics.likesBooks);
            model.LikesMovies = Utilities.mapStatusEnumToWord(statistics.likesMovies);
            model.LikesMusic = Utilities.mapStatusEnumToWord(statistics.likesMusic);
            model.LikesTravelling = Utilities.mapStatusEnumToWord(statistics.likesTraveling);
            model.LikesSports = Utilities.mapStatusEnumToWord(statistics.likesSports);
            model.IsSportsman = Utilities.mapStatusEnumToWord(statistics.isSportsman);
            model.SortedFriendsLikesCategoriesFrequency = statistics.SortedFriendsLikesCategoriesFrequency.Take(10);
            model.SortedFriendsLikesFrequency = statistics.SortedFriendsLikesFrequency.Take(10);
            model.SortedUserLikesCategoriesFrequency = statistics.SortedUserLikesCategoriesFrequency.Take(10);
            model.SortedSimillarFriends = statistics.SortedSimillarFriends.Take(10);
            model.SortedFriendsCheckinsFrequency = statistics.SortedFriendsCheckinsFrequency.Take(10);
            model.SortedUserCheckinsFrequency = statistics.SortedUserCheckinsFrequency.Take(10);
            return model;
        }

        private FoursquareStatisticsModel prepareFoursquareStatisticsModel(FoursquareStatistics statistics)
        {
            FoursquareStatisticsModel model = new FoursquareStatisticsModel();

            model.SortedCheckinCategoriesFrequency = statistics.SortedCheckinCategoriesFrequency;
            model.SortedCheckinsFrequency = statistics.SortedCheckinsFrequency;
            model.SortedFriendsCheckinCategoriesFrequency = statistics.SortedFriendsCheckinCategoriesFrequency;
            model.SortedFriendsCheckinsFrequency = statistics.SortedFriendsCheckinsFrequency;
            model.SortedVisitedCitiesFrequency = statistics.SortedVisitedCitiesFrequency;
            model.Name = statistics.Name;

            return model;
        }

       

        #endregion
    }
}
