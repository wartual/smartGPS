using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using smartGPS.Areas.Administration.Models;
using smartGPS.Business;
using smartGPS.Business.ExternalServices;
using smartGPS.Business.Models;

namespace smartGPS.Areas.Dashboard.Controllers
{
    public class ExternalServicesController : BaseDashboardController
    {

        #region Importd data

        [AllowAnonymous]
        public ActionResult ExternalLoginsList(String provider)
        {
            ViewBag.Provider = provider;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [AllowAnonymous]
        public ActionResult FacebookLogin()
        {
            return PartialView("_FacebookLogin");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

           [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;
            int result;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }


            if (ModelState.IsValid)
            {
              
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {

            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return View("ExternalLoginFailure");
            }
            
           
            if(result.Provider.ToLower().Equals("facebook"))
            {
                Boolean managementResult = FacebookManagement.importFacebookData(result.ProviderUserId, User.Identity.Name);
                if(!managementResult)
                    return View("ExternalLoginFailure");
                else
                    return RedirectToAction("FacebookProfile");
            }

            return View();
       }

        public ActionResult ExternalLoginFailure()
        {
            return View("ExternalLoginFailure");
        }

        #endregion

        #region Facebook

        public ActionResult GetFacebookData(String token)
        {
            Boolean success = FacebookManagement.importFacebookData(token, User.Identity.Name);
            if (success)
                return RedirectToAction("FacebookProfile");
            else
                return RedirectToAction("ExternalLoginFailure");
        }


        public ActionResult FacebookProfile()
        {
            FacebookProfileModel model = FacebookManagement.getFacebookProfileData(User.Identity.Name);
            return View(model);
        }

      

        #endregion

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
