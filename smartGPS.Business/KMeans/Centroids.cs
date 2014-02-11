using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business.KMeans
{
    public class Centroids
    {
        public List<double> centroid { get; set; }
        public int index { get; set; }
        public List<List<double>> elements { get; set; }

        public Centroids(List<double> centroid, int index, List<List<double>> elements)
        {
            this.centroid = centroid;
            this.index = index;
            this.elements = elements;
        }
    }
}