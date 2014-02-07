using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using smartGPS.Areas.Administration.Models;
using smartGPS.Areas.Dashboard.Models;
using smartGPS.Business;
using smartGPS.Business.ExternalServices;
using smartGPS.Business.Models;
using smartGPS.Business.Models.Facebook;

namespace smartGPS.Areas.Dashboard.Controllers
{
    public class ExternalServicesController : BaseDashboardController
    {

        #region Imported data

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

        public ActionResult FoursquareLogin()
        {
            smartGPS.Persistance.FoursquareProfile profile = FourqsquareManagement.getFoursquareProfileData(User.Identity.Name);
            if (profile == null)
            {
                TempData["profile"] = profile;
                return RedirectToAction("ShowFoursquareStatistics", "Profile", new { area = "Dashboard" });
            }
            else
            {
                return PartialView("_FoursquareLogin");
            }
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
            
           
            //if(result.Provider.ToLower().Equals("facebook"))
            //{
            //    Boolean managementResult = FacebookManagement.importFacebookData(result.ProviderUserId, User.Identity.Name);
            //    if(!managementResult)
            //        return View("ExternalLoginFailure");
            //    else
            //        return RedirectToAction("FacebookProfile");
            //}

            return View();
       }

        public ActionResult ExternalLoginFailure()
        {
            return View("ExternalLoginFailure");
        }

        #endregion


        #region Facebook
        [HttpGet]
        public ActionResult FacebookProfile(FacebookStatisticsModel model)
        {
            ViewBag.LoadData = false;
             return View();
        }
        
        [HttpPost]
        public ActionResult FacebookProfile(String dummy)
        {
            ViewBag.LoadData = true;
            return View();
        }

        #endregion


        #region Foursquare
        [HttpGet]
        public ActionResult FoursquareProfile()
        {
            ViewBag.LoadData = false;
            return View();
        }

        [HttpPost]
        public ActionResult FoursquareProfile(String dummy)
        {
            String url = APICalls.getFoursquareAuthTokenUrl();
            return Redirect(url);
        }

        [HttpGet]
        public ActionResult FoursquareResponse(String code)
        {
            Boolean status = FourqsquareManagement.obtainAuthToken(code, User.Identity.Name);
            if (status)
            {
                return RedirectToAction("ShowFoursquareStatistics", "Profile", new { area = "Dashboard" });
            }
            else
            {
                return RedirectToAction("ExternalLoginFailure");
            }
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
