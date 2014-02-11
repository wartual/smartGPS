using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class CBATestDa:BaseClass
    {

        public static IEnumerable<CBATest> cbaTest_getAll()
        {
            return db.CBATest;
        }
    }
}