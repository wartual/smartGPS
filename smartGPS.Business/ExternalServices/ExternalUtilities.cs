using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeoCoding;
using GeoCoding.Google;
using GeoCoding.Yahoo;

namespace smartGPS.Business.ExternalServices
{
    public class ExternalUtilities
    {

        public static IEnumerable<Address> getAddressesFromGpsCoordinates(double latitude, double longitude)
        {
            IGeoCoder geoCoder = new GoogleGeoCoder();
            IEnumerable<Address> addresses = geoCoder.ReverseGeocode(latitude, longitude);
            return addresses;
        }
    }
}