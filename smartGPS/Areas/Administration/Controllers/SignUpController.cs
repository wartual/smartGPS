using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartGPS.Areas.Administration.Models;
using smartGPS.Business;

namespace smartGPS.Areas.Administration.Controllers
{
    public class SignUpController : Controller
    {
        //
        // GET: /Administration/SignUp/

        public ActionResult Index()
        {
            SignUpModel viewModel = new SignUpModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                int result = UserAdministration.signUp(model.username, model.password, model.name, model.surname);
                // model is valid, try to sign up user
                if (result == (int)ErrorHandler.SignUpErrors.Success)
                {
                    return RedirectToAction("Index", "SignIn");
                }
                else
                {
                    if (result == (int)ErrorHandler.SignUpErrors.UsernameAlreadyTaken)
                    {
                        ModelState.AddModelError(string.Empty, "Username is already taken!");
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
