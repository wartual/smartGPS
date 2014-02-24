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
using smartGPS.Business.Custom;
using smartGPS.Business.ExternalServices;
using smartGPS.Business.Models;
using smartGPS.Business.Models.Foursquare;
using smartGPS.Models.UserAdministration;
using smartGPS.Persistance;

namespace smartGPS.Controllers.API
{
    public class TravelController : BaseAPIController
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
                    foreach (var item in travels)
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
                                                model.departureAddress, model.departureLatitude, model.departureLongitude, model.time, model.distance, null);
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
            Haversine haversine = new Haversine();
            List<SmartNode> nodes = pathSearch.search(model.departureLatitude, model.departureLongitude, model.destinationLatitude, model.destinationLongitude, 11);
            SmartLocation lastLocation = new SmartLocation(0, 0);

            List<SmartNode> directions = new List<SmartNode>();
            directions.Add(nodes.ElementAt(0));
            FoursquareExploreVenueResponse venues;
            SmartNode node;
        
            for (int i = 0; i < nodes.Count; i++)
            {
                node = nodes.ElementAt(i);
                if (haversine.Distance(new SmartLocation(node.latitude, node.longitude), lastLocation, Haversine.DistanceType.Kilometers) > 2.5)
                {
                    venues = FourqsquareManagement.getExploreVenuesByCategories(node.latitude, node.longitude, model.userId, 100);
                    foreach (GroupItems item in venues.Response.Groups.ElementAt(0).Items)
                    {
                        List<SmartNode> tmpNodes = pathSearch.search(directions.Last().latitude, directions.Last().longitude, item.Venue.Location.Latitude, item.Venue.Location.Longitude, 11);
                        SmartNode venue = tmpNodes.Last();

                        tmpNodes.RemoveAt(tmpNodes.Count() - 1);

                        directions.AddRange(tmpNodes);
                        directions.Add(new SmartNode(venue.latitude, venue.longitude, venue.id , "foursquare"));
                        // directions.Add(new SmartNode(item.Venue.Location.Latitude, item.Venue.Location.Longitude, "foursquare"));
                    }

                    // return to next point
                    List<SmartNode> returnPath;
                    if (i + 1 < nodes.Count)
                    {
                        returnPath = pathSearch.search(directions.Last().latitude, directions.Last().longitude, nodes.ElementAt(i + 1).latitude, nodes.ElementAt(i + 1).longitude, 11);
                    }
                    else
                    {
                        returnPath = pathSearch.search(directions.Last().latitude, directions.Last().longitude, node.latitude, node.longitude, 11);

                    }
                    directions.AddRange(returnPath);

                    lastLocation.Latitude = node.latitude;
                    lastLocation.Longitude = node.longitude;
                }
                else
                {
                    directions.Add(node);
                }
            }

            String json = JsonConvert.SerializeObject(directions);
            String travel = JsonConvert.SerializeObject(model);

            String id = TravelManager.newTravel(model.userId, model.statusId, model.destinationAddress, model.currentLatitude, model.currentLongitude, model.destinationLatitude, model.destinationLongitude,
                                             model.departureAddress, model.departureLatitude, model.departureLongitude, model.time, model.distance, json);
          
            smartGPS.Persistance.User user = UserAdministration.getUserByUserId(model.userId);
            NotificationsManager.sendNodes(user.GcmId, id, travel);
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