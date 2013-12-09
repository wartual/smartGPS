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
using smartGPS.Persistance.UsersFolder;

namespace smartGPS.Areas.API.Controllers
{
    public class UserController : BaseAPIController
    {

        [HttpPut]
        [ActionName("loginUser")]
        public HttpResponseMessage GetLoginDetails([FromBody] ExternalLoginModel model)
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
        public HttpResponseMessage CreateNewUser([FromBody] ExternalNewUserModel model)
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, response);
            }
        }
    }
}
