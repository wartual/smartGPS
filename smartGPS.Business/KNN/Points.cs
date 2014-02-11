using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business.KNN
{
    public class Points
    {
        public List<double> elements { get; set; }
        public int index { get; set; }
        public String category { get; set; }

        public Points(List<double> elements, int index, String category)
        {
            this.elements = elements;
            this.index = index;
            this.category = category;
        }
    }
}