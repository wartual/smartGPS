using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using smartGPS.API.Models;

namespace smartGPS.API.Controllers
{
    public class UsersController : ApiController
    {
        public HttpResponseMessage GetUser(String id)
        {
            SmartJSONResponseModel model = new SmartJSONResponseModel();
            model.Message = "SEKSI";
            model.Status = "NOUP";
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }
    }
}
