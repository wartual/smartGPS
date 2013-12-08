using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using smartGPS.API.Custom;
using smartGPS.API.Models;
using smartGPS.API.Models.UserAdministration;
using smartGPS.Business;

namespace smartGPS.API.Controllers
{
    [RoutePrefix("api/users")]
    public class UserAdministrationController : ApiController
    {
        [HttpGet]
        [Route("loginUser")]
        public HttpResponseMessage LoginUser(String username, String password)
        {
            SmartJsonResponseModel response = new SmartJsonResponseModel();

            smartGPS.Persistance.User user = UserAdministration.getUserByUserId(username);
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
