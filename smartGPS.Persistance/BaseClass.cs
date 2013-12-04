using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class BaseClass
    {
        protected static smartGPSEntities db = new smartGPSEntities();
    }
}