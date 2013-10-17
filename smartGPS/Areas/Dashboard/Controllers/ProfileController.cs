using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using smartGPS.Areas.Administration.Models;
using smartGPS.Areas.Dashboard.Models;
using smartGPS.BusinessLogic;
using smartGPS.Persistence;

namespace smartGPS.Areas.Dashboard.Controllers
{
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
