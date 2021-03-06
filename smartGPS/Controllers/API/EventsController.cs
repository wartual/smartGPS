﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using smartGPS.Areas.API.Models;
using smartGPS.Business;
using smartGPS.Business.ExternalServices;
using smartGPS.Business.Models.Foursquare;
using smartGPS.Models.UserAdministration;

namespace smartGPS.Controllers.API
{
    public class EventsController : BaseAPIController
    {
        [HttpGet]
        [ActionName("getEvents")]
        public HttpResponseMessage getEvents([FromUri] APIGetEventsModel model)
        {
            try
            {
                if (UserAdministration.getUserByUserId(model.userId) == null)
                {
                    response.Status = SmartResponseType.RESULT_FAIL;
                    response.Message = "User does not exists";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    FoursquareExploreVenueResponse events = FourqsquareManagement.getExploreVenues(model.latitude, model.longitude);
                    if (events != null)
                    {
                        if (model.num != 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, events.Response.Groups.First().Items.Take(model.num));

                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, events.Response.Groups.First().Items);
                        }
                    }
                    else
                    {
                        response.Status = SmartResponseType.RESULT_FAIL;
                        response.Message = "Search for places was unsuccessful!";
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
                    }
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
    }
}
