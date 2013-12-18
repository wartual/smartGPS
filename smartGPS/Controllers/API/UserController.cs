using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using smartGPS.Areas.API.Models;
using smartGPS.Areas.API.Models.UserAdministration;
using smartGPS.Business;
using smartGPS.Controllers;
using smartGPS.Models.UserAdministration;
using smartGPS.Persistance;
using smartGPS.Persistance.UsersFolder;

namespace smartGPS.Areas.API.Controllers
{
    public class UserController : BaseAPIController
    {

        [HttpPut]
        [ActionName("loginUser")]
        public HttpResponseMessage GetLoginDetails([FromBody] APIExternalLoginModel model)
        {
            try
            {
                smartGPS.Persistance.User user = UserAdministration.getUserByUsername(model.username);
                if (user != null && Utilities.encryptPassword(model.password) == user.Password)
                {
                    response.Status = SmartResponseType.RESULT_OK;
                    response.Message = user.Id;
                    UserAdministration.APIexternaLoginUpdateDateLogin(user);
                }
                else
                {
                    response.Status = SmartResponseType.RESULT_FAIL;
                    response.Message = "Username or password is not valid!";
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                response.Status = SmartResponseType.RESULT_FAIL;
                response.Message = "An error has occured!";
                return Request.CreateResponse(HttpStatusCode.BadRequest, response);
            }
        }


        [HttpPost]
        [ActionName("registerUser")]
        public HttpResponseMessage CreateNewUser([FromBody] APINewUserModel model)
        {
            try
            {
                int result = UserAdministration.signUp(model.username, model.password, model.name, model.surname, false, null, null);

                if (result == (int)ErrorHandler.SignUpErrors.Success)
                {
                    smartGPS.Persistance.User user = UserAdministration.getUserByUsername(model.username);
                    response.Status = SmartResponseType.RESULT_OK;
                    response.Message = user.Id;
                    UserAdministration.APIexternaLoginUpdateDateLogin(user);

                    return Request.CreateResponse(HttpStatusCode.Created, response);
                }
                else
                {
                    if (result == (int)ErrorHandler.SignUpErrors.UsernameAlreadyTaken)
                    {
                        response.Status = SmartResponseType.RESULT_FAIL;
                        response.Message = "Username is already taken!";
                    }
                    else
                    {
                        response.Status = SmartResponseType.RESULT_FAIL;
                        response.Message = "An error has occured while saving to database";
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                
            }
            catch (Exception e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                response.Status = SmartResponseType.RESULT_FAIL;
                response.Message = "An error has occured!";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }


        [HttpGet]
        [ActionName("getUser")]
        public HttpResponseMessage GetUserProfile(String userId)
        {
            try
            {
                smartGPS.Persistance.User user = UserAdministration.getUserByUserId(userId);

                if (user == null)
                {
                    response.Status = SmartResponseType.RESULT_FAIL;
                    response.Message = "User does not exists";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, getProfileModel(UserAdministration.getProfileByUserId(userId)));
                }
            }
            catch (Exception e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                response.Status = SmartResponseType.RESULT_FAIL;
                response.Message = "An error has occured!";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }


        [HttpPut]
        [ActionName("updateProfile")]
        public HttpResponseMessage UpdateUserProfile([FromBody] APIProfileModel model)
        {
            try
            {
                Boolean status = UserAdministration.updateProfile(model.UserId, model.Username, model.Name, model.Surname, model.DateOfBirth,
                                                                    model.Gender, model.Email, model.Phone, model.Address, model.PostalOffice, model.Country, null);
                if (status)
                {
                    response.Status = SmartResponseType.RESULT_OK;
                    response.Message = "User profile has been updated!";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    response.Status = SmartResponseType.RESULT_FAIL;
                    response.Message = "User does not exist!";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                response.Status = SmartResponseType.RESULT_FAIL;
                response.Message = "An error has occured!";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        #region Utils

        private APIProfileModel getProfileModel(Profile profile)
        {
            APIProfileModel model = new APIProfileModel();
            model.Address = profile.Address;
            model.Country = profile.Country;
            if (profile.DateOfBirth.HasValue)
            {
                model.DateOfBirth = Utilities.ToEpochFromDateTime(profile.DateOfBirth.Value);
            }
            model.Email = profile.Email;
            model.Gender = profile.Gender;
            model.Name = profile.Name;
            model.PostalOffice = profile.PostalOffice;
            model.Surname = profile.Surname;
            model.Phone = profile.Phone;
            model.UserId = profile.UserId;
            model.Username = profile.User.Username;

            return model;
        }

        #endregion
    }
}
