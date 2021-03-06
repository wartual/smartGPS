﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using smartGPS.Areas.API.Models;
using smartGPS.Business;
using smartGPS.Models.UserAdministration;
using smartGPS.Persistance;

namespace smartGPS.Controllers.API
{
    public class NotificationController : BaseAPIController
    {
        [HttpPut]
        [ActionName("updateGcmId")]
        public HttpResponseMessage updateGcmId([FromBody] APIUserGcm model)
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
                    UserAdministration.updateUsersGcmId(model.userId, model.gcmId);
                    response.Status = SmartResponseType.RESULT_OK;
                    response.Message = "User GCM id updated";
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
        [ActionName("testNotification")]
        public HttpResponseMessage testNotifications(String text)
        {
            try
            {
                NotificationsManager.sendToAllUsers("Ovo je test");
                response.Status = SmartResponseType.RESULT_OK;
                response.Message = "User GCM id updated";
                return Request.CreateResponse(HttpStatusCode.OK, response);
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
        [ActionName("getNotificationCategories")]
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
                    List<NotificationCategory> categories = NotificationsManager.getNotificationCategories();
                    List<APINotificationCategories> apiCategories = new List<APINotificationCategories>();
                    foreach (NotificationCategory model in categories)
                    {
                        apiCategories.Add(mapToAPINotificationCategories(model));
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, apiCategories);
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
        [ActionName("createNotification")]
        public HttpResponseMessage createNotification([FromBody] APIAddNotification notification)
        {
            try
            {
                if (UserAdministration.getUserByUserId(notification.userId) == null)
                {
                    response.Status = SmartResponseType.RESULT_FAIL;
                    response.Message = "User does not exists";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    String id = NotificationsManager.addNew(notification.text, notification.category, notification.latitude, notification.longitude, notification.userId, notification.address);
                    NotificationsManager.notifyUsersNearNotification(id, notification.userId);
                    response.Status = SmartResponseType.RESULT_OK;
                    response.Message = "Notification saved!";
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
        [ActionName("getNotificationsNearLocation")]
        public HttpResponseMessage getNotificationsNearLocation(String userId, double latitude, double longitude)
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
                    List<Notifications> notifications = NotificationsManager.getNotificationsNearLocation(latitude, longitude);
                    List<APIAddNotification> apiNotifications = new List<APIAddNotification>();

                    foreach (Notifications item in notifications)
                    {
                        apiNotifications.Add(mapToApiAddNotification(item));
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, apiNotifications);
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
        [ActionName("deactivateNotification")]
        public HttpResponseMessage deactivateNotification([FromBody] APIDeactivateNotification model)
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
                    NotificationsManager.deactivateNotification(model.notificationId);

                    response.Status = SmartResponseType.RESULT_OK;
                    response.Message = "Notification deactivated!";
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
        [ActionName("rateNotification")]
        public HttpResponseMessage rateNotification([FromBody] APIRateNotification rateNotification)
        {
            try
            {
                if (UserAdministration.getUserByUserId(rateNotification.userId) == null)
                {
                    response.Status = SmartResponseType.RESULT_FAIL;
                    response.Message = "User does not exists";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else if(rateNotification.thumbsDown && rateNotification.thumbsUp)
                {
                    response.Status = SmartResponseType.RESULT_FAIL;
                    response.Message = "Please choose only once action!";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else if (!rateNotification.thumbsDown && !rateNotification.thumbsUp)
                {
                    response.Status = SmartResponseType.RESULT_FAIL;
                    response.Message = "Please choose one action!";
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    if (rateNotification.thumbsUp)
                    {
                        NotificationsManager.thumbsUp(rateNotification.notificationId);
                    }
                    else
                    {
                        NotificationsManager.thumbsDown(rateNotification.notificationId);
                    }
                    response.Status = SmartResponseType.RESULT_OK;
                    response.Message = "Notification rated!";
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

        private APIAddNotification mapToApiAddNotification(Notifications model)
        {
            APIAddNotification api = new APIAddNotification();

            api.category = model.CategoryId;
            api.dateCreated = Utilities.ToEpochFromDateTime(model.DateCreated);
            api.latitude = model.Latitude;
            api.longitude = model.Longitude;
            api.text = model.Text;
            api.userId = model.UserId;
            api.username = model.User.Username;
            api.address = model.Address.Replace(" ", "+");
            api.notificationId = model.Id;
            api.thumbsUp = model.ThumbsUp;
            api.thumbsDown = model.ThumbsDown;
            return api;
        }

        private APINotificationCategories mapToAPINotificationCategories(NotificationCategory model)
        {
            APINotificationCategories api = new APINotificationCategories();

            api.id = model.Id;
            api.type = model.Type;

            return api;
        }

        #endregion
    }
}
