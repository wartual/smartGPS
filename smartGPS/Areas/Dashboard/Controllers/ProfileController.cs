using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartGPS.Areas.Administration.Models;
using smartGPS.Areas.Dashboard.Models;
using smartGPS.Business;
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
            ProfileModel viewModel = Mapping.usersToProfileModel(profile);


            return View(viewModel);
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
    }
}
