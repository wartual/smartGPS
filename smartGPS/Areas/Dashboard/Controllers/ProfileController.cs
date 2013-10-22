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
    public class ProfileController : Controller
    {
        //
        // GET: /Dashboard/Profile/

        public ActionResult Index()
        {
            profile profile = UserAdministration.getProfile(User.Identity.Name);
            ProfileModel viewModel = Mapping.usersToProfileModel(profile);
            return View(viewModel);
        }

    }
}
