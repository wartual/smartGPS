using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Models.UserAdministration
{
    public class APIGetPlacesModel
    {
        public String userId { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public int num { get; set; }
    }
}