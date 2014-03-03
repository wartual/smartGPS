using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class TravelDao:BaseClass
    {

        #region TravelStatus

        public static IEnumerable<TravelStatusCategory> getAllStatusCategory()
        {
            return db.TravelStatusCategory;
        }

        public static TravelStatusCategory getTravelStatusByType(String status)
        {
            return db.TravelStatusCategory.Where(item => item.Type.Equals(status)).SingleOrDefault();
        }

        #endregion


        #region Travel

        public static void updateTravelsCurrentLocation(Travel travel, double currentLatitude, double currentLongitude)
        {
            travel.CurrentLatitude = currentLatitude;
            travel.CurrentLongitude = currentLongitude;
            travel.DateUpdated = DateTime.Now;

            db.SaveChanges();
        }

        public static void updateTravelsDirections(Travel travel, double currentLatitude, double currentLongitude, String directions)
        {
            travel.CurrentLatitude = currentLatitude;
            travel.CurrentLongitude = currentLongitude;
            travel.Directions = directions;
            travel.DateUpdated = DateTime.Now;

            db.SaveChanges();
        }

        public static IEnumerable<Travel> getByUserId(String userId)
        {
            return db.Travel.Where(item => item.UserId.Equals(userId));
        }

        public static Travel getById(String id)
        {
            return db.Travel.Where(item => item.Id.Equals(id)).SingleOrDefault();
        }

        public static String newTravel(String id, String userId, String statusId, String destinationAddress, double currentLatitude, double currentLongitude, double destinationLatitude, double destinationLongitude,
                                    String departureAddress, double departureLatitude, double departureLongitude, double time, double distance, string directions, DateTime dateCreated, DateTime dateUpdated)
        {
            Travel travel = new Travel();
            travel.DepartudeAddress = departureAddress;
            travel.DepartudeLongitude = departureLongitude;
            travel.DepartureLatitude = departureLatitude;
            travel.DestinationAddress = destinationAddress;
            travel.DestinationLatitude = destinationLatitude;
            travel.DestinationLongitude = destinationLongitude;
            travel.CurrentLatitude = currentLatitude;
            travel.CurrentLongitude = currentLongitude;
            travel.Distance = distance;
            travel.Directions = directions.Trim();
            travel.Id = id;
            travel.StatusId = statusId;
            travel.Time = time;
            travel.UserId = userId;
            travel.DateCreated = dateCreated;
            travel.DateUpdated = dateUpdated;

            db.Travel.Add(travel);
            db.SaveChanges();

            return id;
        }

        public static void updateTravelStatus(String statusId, Travel travel)
        {
            travel.StatusId = statusId;
            travel.DateUpdated = DateTime.Now;

            db.SaveChanges();
        }

        public static IEnumerable<Travel> getAllByStatus(TravelStatusCategory status)
        {
            return db.Travel.Where(item => item.StatusId.Equals(status.Id));
        }
        #endregion
    }
}