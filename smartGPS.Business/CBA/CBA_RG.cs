using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Business.ExternalServices;

namespace smartGPS.Business.CBA
{
    public class CBA_RG
    {

        private double Support;
        private double Confidence;

        ///<summary>
        ///Constructor, needs support and confidence
        ///</summary>
        public CBA_RG(double support, double confidence)
        {
            this.Support = support;
            this.Confidence = confidence;
        }

        public CBA_RG()
        {
            this.Support = Config.CBA_DEFAULT_SUPPORT;
            this.Support = Config.CBA_DEFAULT_CONFIDENCE;
        }


    }
}