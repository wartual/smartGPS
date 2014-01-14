using System;
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
                    response.Status = SmartResponseType.RESULT_FAIL;
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
                List<String> responses = NotificationsManager.sendToAllUsers("Ovo je test");
                return Request.CreateResponse(HttpStatusCode.OK, responses.ToString());
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
                    return Request.CreateResponse(HttpStatusCode.OK, categories);
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
                    NotificationsManager.addNew(notification.text, notification.category, notification.latitude, notification.longitude, notification.userId);
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
    }
}
