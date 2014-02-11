using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business.KMeans
{
    public class Points
    {
        public List<double> elements { get; set; }
        public int centroidIndex { get; set; }

        public Points(List<double> elements, int centroidIndex)
        {
            this.elements = elements;
            this.centroidIndex = centroidIndex;
        }
    }
}