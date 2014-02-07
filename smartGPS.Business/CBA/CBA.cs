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

            Boolean same = Utilities.compareDictionaries(c1.Conditions, c2.Conditions);

            CondSet c3 = new CondSet();
            c3.addCondition("a", "1");
            c3.addCondition("c", "3");

            CondSet c4 = new CondSet();
            c4.addCondition("c", "3");
            c4.addCondition("a", "1");

            same = Utilities.compareDictionaries(c3.Conditions, c4.Conditions);
        }
    }
}