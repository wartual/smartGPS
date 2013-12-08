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

namespace smartGPS.Areas.API.Controllers
{
    public class UserController : BaseAPIController
    {
        
        public HttpResponseMessage GetUser(String username, String password)
        {
            smartGPS.Persistance.User user = UserAdministration.getUserByUsername(username);
            if (user != null && Utilities.encryptPassword(password) == user.Password)
            {
                response.Status = SmartResponseType.RESULT_OK;
                response.Message = user.Id;
            }
            else
            {
                response.Status = SmartResponseType.RESULT_FAIL;
                response.Message = "Username or password is not valid!";
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
