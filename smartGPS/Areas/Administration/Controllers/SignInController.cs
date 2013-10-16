using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartGPS.BusinessLogic;
using smartGPS.Models.Administration;

namespace smartGPS.Areas.Administration.Controllers
{
    public class SignInController : Controller
    {
        //
        // GET: /Administration/Home/

        public ActionResult Index()
        {
            SignInModel model = new SignInModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                int result = UserAdministration.signIn(model.username, model.password, model.remeberMe);
                // model is valid, try to sign up user
                if (result == (int)ErrorHandler.SignUpErrors.Success)
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Dashboard" });
                }
                else
                {
                    if (result == (int)ErrorHandler.SignInErrors.Failed)
                    {
                        ModelState.AddModelError(string.Empty, "Username or password is not correct!");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An error has occured while saving to database");
                    }
                    return View(model);
                }
            }
            else
            {
                // model is not valid, return view
                return View(model);
            }
        }
    }
}
