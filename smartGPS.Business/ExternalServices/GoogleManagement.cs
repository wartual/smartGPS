﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using GeoCoding;
using GeoCoding.Google;
using GeoCoding.Yahoo;
using Newtonsoft.Json;
using smartGPS.Business.Models;
using smartGPS.Business.Models.GoogleModels;
using smartGPS.Persistance;

namespace smartGPS.Business.ExternalServices
{
    public class GoogleManagement
    {

        public static IEnumerable<Address> getLocationsFromGpsCoordinates(double latitude, double longitude)
        {
            IGeoCoder geoCoder = new GoogleGeoCoder();
            IEnumerable<Address> addresses = geoCoder.ReverseGeocode(latitude, longitude);
            return addresses;
        }

        public static IEnumerable<Address> getLocationsFromAddress(String address)
        {
            IGeoCoder geoCoder = new GoogleGeoCoder();
            IEnumerable<Address> addresses = geoCoder.GeoCode(address);
            return addresses;
        }

        public static GoogleGeoCodeResponse getDataFromGoogleApis(String address)
        {
            GoogleGeoCodeResponse model = null;
            String url = null;
            if (address != null)
            {
                url = APICalls.GOOGLE_MAP_APIS + "address=" +  HttpUtility.UrlEncode(address);
            }

            WebRequest request = WebRequest.Create(url);

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        String responseString = new StreamReader(stream).ReadToEnd();
                        model = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(responseString);
                        return model;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static GoogleMapsDirectionsResponse getDataFromGoogleMapsApis(String userId, double startLatitude, double startLongitude, double endLatitude, double endLongitude, String mode)
        {
            GoogleMapsDirectionsResponse model = null;
            String url = APICalls.getMapDirectionsFormattedUrl(startLatitude, startLongitude, endLatitude, endLongitude, mode);

            WebRequest request = WebRequest.Create(url);

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        String responseString = new StreamReader(stream).ReadToEnd();
                        model = JsonConvert.DeserializeObject<GoogleMapsDirectionsResponse>(responseString);
                        return model;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static GooglePlacesResponse getDataFromGooglePlaces(double latitude, double longitude)
        {
            GooglePlacesResponse model = null;
            String url = APICalls.getPlacesFormattedUrl(latitude, longitude, Config.PLACES_RADIUS);

            WebRequest request = WebRequest.Create(url);

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        String responseString = new StreamReader(stream).ReadToEnd();
                        model = JsonConvert.DeserializeObject<GooglePlacesResponse>(responseString);
                        return model;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static GooglePlacesResponse getDataFromGooglePlacesByText(String text)
        {
            GooglePlacesResponse model = null;
            String url = APICalls.getPlacesByTextFormattedUrl(text, Config.PLACES_RADIUS);

            WebRequest request = WebRequest.Create(url);

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        String responseString = new StreamReader(stream).ReadToEnd();
                        model = JsonConvert.DeserializeObject<GooglePlacesResponse>(responseString);
                        return model;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static GooglePlacesResponse getDataFromGooglePlacesByFilter(String userId, double latitude, double longitude)
        {
            Profile profile = UserAdministration.getProfileByUserId(userId);

            if (profile == null || profile.Category == null)
            {
                return null;
            }
            else
            {
                GooglePlacesResponse model = null;
                String url = APICalls.getPlacesFormattedUrlByFilters(latitude, longitude, Config.PLACES_RADIUS, profile.Category.Value);

                WebRequest request = WebRequest.Create(url);

                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            String responseString = new StreamReader(stream).ReadToEnd();
                            model = JsonConvert.DeserializeObject<GooglePlacesResponse>(responseString);
                            return model;
                        }
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }

        }
    }
}