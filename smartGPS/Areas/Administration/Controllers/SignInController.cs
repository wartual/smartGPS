using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using smartGPS.Areas.Administration.Models;
using smartGPS.Business;

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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if(UserAdministration.signInExternalUser(result.UserName) == (int)ErrorHandler.SignInErrors.Database)
            {
                   return RedirectToAction("ExternalLoginFailure");
            }
            else if(UserAdministration.signInExternalUser(result.UserName) == (int)ErrorHandler.SignInErrors.Database)
            {
                 return RedirectToAction("Index", "Dashboard", new { area = "Dashboard" }); 
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }


            if (ModelState.IsValid)
            {

                int result = UserAdministration.signUp(model.UserName, providerUserId, "", "", true);
                // model is valid, try to sign up user
                if (result == (int)ErrorHandler.SignUpErrors.Success)
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Dashboard" });  
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

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            return View(model);
        }

        #region Custom

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        #endregion
    }
}
