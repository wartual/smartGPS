using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using smartGPS.Areas.API.Models.UserAdministration;

namespace smartGPS.Controllers
{
    public class BaseAPIController : ApiController
    {
        protected SmartJsonResponseModel response = new SmartJsonResponseModel();
        protected HttpStatusCode statusCode = new HttpStatusCode();
    }
}
