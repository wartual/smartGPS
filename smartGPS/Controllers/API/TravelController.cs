using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Newtonsoft.Json;
using smartGPS.Areas.API.Models;
using smartGPS.Business;
using smartGPS.Business.AStar;
using smartGPS.Business.Models;
using smartGPS.Models.UserAdministration;
using smartGPS.Persistance;

namespace smartGPS.Controllers.API
{
    public class TravelController:BaseAPIController
    {

        [HttpGet]
        [ActionName("getTravelStatusCategories")]
        public HttpResponseMessage getCategories([FromUri] String userId)
        {
            try
            {
                if (UserAdministration.getUserByUserId(userId) == null)
                {
                    response.Status = SmartResponseType.RESULT_FAIL;
                    response.Message = "User does not exists";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    List<TravelStatusCategory> categories = TravelManager.getAllTravelStatus();
                    List<APITravelStatusCategory> api = new List<APITravelStatusCategory>();
                    foreach (var item in categories)
                    {
                        api.Add(mapToAPITravelStatusCategory(item));
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, api);
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
        [ActionName("getTravelsForUser")]
        public HttpResponseMessage getTravelForUser([FromUri] String userId)
        {
            try
            {
                if (UserAdministration.getUserByUserId(userId) == null)
                {
                    response.Status = SmartResponseType.RESULT_FAIL;
                    response.Message = "User does not exists";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    List<Travel> travels = TravelManager.getAllByUserId(userId);
                    List<APITravel> apiTravel = new List<APITravel>();
                    foreach(var item in travels)
                    {
                        apiTravel.Add(mapToAPITravel(item));
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, apiTravel);
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
        [ActionName("updateTravelStatus")]
        public HttpResponseMessage updateTravelStatus([FromBody] APITravelStatus model)
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
                    TravelManager.updateTravelStatus(model.travelId, model.travelStatusId);
                    response.Status = SmartResponseType.RESULT_OK;
                    response.Message = "Travel status updated!";
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

        [HttpPut]
        [ActionName("updateTravelsCurrentLocation")]
        public HttpResponseMessage updateTravelsCurrentLocation([FromBody] APITravelsCurrentLocation model)
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
                    TravelManager.updateTravelsCurrentLocation(model.travelId, model.userId, model.latitude, model.longitude);
                    response.Status = SmartResponseType.RESULT_OK;
                    response.Message = "Travel status updated!";
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

        [HttpPost]
        [ActionName("newTravel")]
        public HttpResponseMessage newTravel([FromBody] APITravel model)
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
                    String id = TravelManager.newTravel(model.userId, model.statusId, model.destinationAddress, model.currentLatitude, model.currentLongitude, model.destinationLatitude, model.destinationLongitude,
                                                model.departureAddress, model.departureLatitude, model.departureLongitude, model.time, model.distance);
                    response.Status = SmartResponseType.RESULT_OK;
                    response.Message = id;
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

        [HttpPost]
        [ActionName("newTravelWithDirections")]
        public HttpResponseMessage newTravelWithDirections([FromBody] APITravel model)
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
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        getDirections(model);
                    }).Start();

                    response.Status = SmartResponseType.RESULT_OK;
                    response.Message = "Search has started!";
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

        private void getDirections(APITravel model)
        {
            PathSearch pathSearch = new PathSearch();
            List<SmartNode> nodes = pathSearch.search(model.departureLatitude, model.departureLongitude, model.destinationLatitude, model.destinationLongitude, 11);

            String json = JsonConvert.SerializeObject(nodes);
            String travel = JsonConvert.SerializeObject(model);
            User user = UserAdministration.getUserByUserId(model.userId);
            NotificationsManager.sendNodes(user.GcmId, json, travel);
        }

        private APITravel mapToAPITravel(Travel model)
        {
            APITravel api = new APITravel();

            api.departureAddress = model.DepartudeAddress;
            api.departureLatitude = model.DepartureLatitude;
            api.departureLongitude = model.DepartudeLongitude;
            api.destinationAddress = model.DestinationAddress;
            api.destinationLatitude = model.DestinationLatitude;
            api.destinationLongitude = model.DestinationLongitude;
            api.currentLatitude = model.CurrentLatitude;
            api.currentLongitude = model.CurrentLongitude;
            api.statusId = model.StatusId;
            api.userId = model.UserId;
            api.id = model.Id;
            api.distance = model.Distance;
            api.time = model.Time;
            api.dateCreated = Utilities.ToEpochFromDateTime(model.DateCreated);
            api.dateUpdated = Utilities.ToEpochFromDateTime(model.DateUpdated);

            return api;
        }

        private APITravelStatusCategory mapToAPITravelStatusCategory(TravelStatusCategory model)
        {
            APITravelStatusCategory api = new APITravelStatusCategory();

            api.id = model.Id;
            api.type = model.Type;

            return api;
        }

        #endregion
    }
}