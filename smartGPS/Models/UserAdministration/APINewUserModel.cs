using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Models.UserAdministration
{
    public class APINewUserModel
    {

        public String username { get; set; }

        public String password { get; set; }

        public String name { get; set; }

        public String surname { get; set; }
    }
}