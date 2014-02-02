using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Business.Custom;
using smartGPS.Persistance;

namespace smartGPS.Business
{
    public class TravelManager
    {
        private static String STATUS_ACTIVE = "Active";

        #region TravelStatus

        public static void updateTravelStatus(String travelId, String travelStatusId)
        {
            Travel model = getById(travelId);

            TravelDao.updateTravelStatus(travelStatusId, model);
        }

        public static TravelStatusCategory getTravelStatusByType(String type)
        {
            return TravelDao.getTravelStatusByType(type);
        }

        public static List<Travel> getAllByStatus(TravelStatusCategory status)
        {
            return TravelDao.getAllByStatus(status).ToList<Travel>();
        }

        public static Travel getById(String travelId)
        {
            return TravelDao.getById(travelId);
        }

        public static List<TravelStatusCategory> getAllTravelStatus()
        {
            return TravelDao.getAllStatusCategory().ToList<TravelStatusCategory>();
        }

        public static List<Travel> getAllByUserId(String userId)
        {
            return TravelDao.getByUserId(userId).ToList<Travel>();
        }

        public static String newTravel(String userId, String statusId, String destinationAddress, double currentLatitude, double currentLongitude, double destinationLatitude, double destinationLongitude,
                                    String departureAddress, double departureLatitude, double departureLongitude, double time, double distance)
        {

            String id = TravelDao.newTravel(Guid.NewGuid().ToString(), userId, statusId, destinationAddress, currentLatitude, currentLongitude, destinationLatitude, destinationLongitude, departureAddress, departureLatitude,
                                        departureLongitude, time, distance, DateTime.Now, DateTime.Now);
            return id;
        }

        public static void updateTravelsCurrentLocation(String travelId, String userId, double currentLatitude, double currentLongitude)
        {
            Travel travel = getById(travelId);

            if(travel != null)
            {
                TravelDao.updateTravelsCurrentLocation(travel, currentLatitude, currentLongitude);
                UserAdministration.updateUserLocation(currentLatitude, currentLongitude, userId);
            }
            
        }

        public static List<User> getAllUsersNearLocation(double latitude, double longitude, double radius, String userId)
        {
            TravelStatusCategory status = getTravelStatusByType(STATUS_ACTIVE);
            //List<Travel> travels = getAllByStatus(status).Where(item => !item.UserId.Equals(userId)).ToList<Travel>();
            List<Travel> travels = getAllByStatus(status).ToList<Travel>();
            if (travels == null)
                return null;
            else
            {
                double distance;
                Haversine haversine = new Haversine();
                SmartLocation notification = new SmartLocation();
                notification.Latitude = latitude;
                notification.Longitude = longitude;
                SmartLocation currentLocation = new SmartLocation();
                List<User> users = new List<User>();
                foreach (Travel travel in travels)
                {
                    currentLocation.Latitude = travel.CurrentLatitude;
                    currentLocation.Longitude = travel.CurrentLongitude;

                    distance = haversine.Distance(notification, currentLocation, Haversine.DistanceType.Kilometers);
                    if (distance <= radius)
                    {
                        users.Add(UserAdministration.getUserByUserId(travel.UserId));
                    }
                }

                return users;
            }
        }

        #endregion
    }
}