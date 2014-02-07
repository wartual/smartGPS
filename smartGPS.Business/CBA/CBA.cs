using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Business.CBA.Models;

namespace smartGPS.Business.CBA
{
    public class CBA
    {

        public static void testConditionSet()
        {
            CondSet c1 = new CondSet();
            c1.addCondition("a", "1");
            c1.addCondition("c", "3");

            CondSet c2 = new CondSet();
            c2.addCondition("b", "2");
            c2.addCondition("a", "1");

            CondSet merged = c1.merge(c2, 3);
        }
    }
}