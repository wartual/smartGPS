using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class BaseClass
    {
        protected static smartgpsEntities db = new smartgpsEntities();
    }
}