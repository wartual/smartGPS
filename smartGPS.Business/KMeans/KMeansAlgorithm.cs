using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MathNet.Numerics.Statistics;
using smartGPS.Business.ExternalServices;
using smartGPS.Business.KMeans;
using smartGPS.Persistance;

namespace smartGPS.Business.KMeansAlgorithm
{
    public class KMeansAlgorithm
    {

        private String userId { get; set; }
        private int k { get; set; }
        private List<Points> dataSet { get; set; }
        private List<Centroids> centroids { get; set; }

        public KMeansAlgorithm(String userId, int k)
        {
            this.userId = userId;
            this.k = k;
            this.dataSet = new List<Points>();
            this.centroids = new List<Centroids>();
        }

        public KMeansAlgorithm(String userId)
        {
            this.userId = userId;
            this.k = Config.KMEANS_DEFAULT_K;
            this.dataSet = new List<Points>();
            this.centroids = new List<Centroids>();
        }

        private void prepareData()
        {
            IEnumerable<FacebookProccesedEntries> entries = FacebookDao.ProccessedFacebookEntries_getAllByUser(userId);
            int i = 0;
            foreach (FacebookProccesedEntries entry in entries)
            {
                List<double> element = new List<double>();
                element.Add(Utilities.mapWordToStatusEnum(entry.LikesBooks));
                element.Add(Utilities.mapWordToStatusEnum(entry.LikesMovies));
                element.Add(Utilities.mapWordToStatusEnum(entry.LikesMusic));
                element.Add(Utilities.mapWordToStatusEnum(entry.LikesSports));
                element.Add(Utilities.mapWordToStatusEnum(entry.LikesTravelling));
                element.Add(Utilities.mapWordToStatusEnum(entry.Sportsman));

                Points point = new Points(element, -1);
                dataSet.Add(point);
            }

            Random random = new Random();
            int index = random.Next(0, entries.Count());
            for (int j = 0; j < k; k++)
            {
                for (int m = 0; k < centroids.Count; m++)
                {
                    index = random.Next(0, entries.Count());
                    if (centroids.ElementAt(m).index == index)
                        continue;
                    else
                        break;
                }

                Centroids centroid = new Centroids(dataSet.ElementAt(index).elements, index, new List<List<double>>());
                centroids.Add(centroid);
            }
        }

        public void runAlgorithm()
        {
            prepareData();

            double correlationCoef;
            double minCorrelation = -1;
            int minCorrelationIndex = 0;
            Boolean change = true;
            int n = 0;

            while (change && n < 5)
            {
                emptyCentroids();

                foreach (Points point in dataSet)
                {
                    for (int i = 0; i < centroids.Count(); i++)
                    {
                        correlationCoef = Correlation.Pearson(point.elements, centroids.ElementAt(i).centroid);
                        if (correlationCoef < minCorrelation)
                        {
                            minCorrelation = correlationCoef;
                            minCorrelationIndex = i;
                        }
                    }

                    centroids.ElementAt(minCorrelationIndex).elements.Add(point.elements);
                    if (point.centroidIndex != minCorrelationIndex)
                        change = true;
                }
            }

            recalculateCentroids();
        }

        private void emptyCentroids()
        {
            for (int i = 0; i < centroids.Count(); i++)
            {
                centroids.ElementAt(i).elements.Clear();
            }
        }

        private void recalculateCentroids()
        {
            Centroids centroid;
            for (int i = 0; i < centroids.Count(); i++)
            {
                centroid = centroids.ElementAt(i);
                List<double> newCentroid = new List<double>();
                List<double> elements;

                for (int k = 0; k < 5; k++)
                {
                    elements = centroid.elements.ElementAt(k);
                    elements.Sort();
                    newCentroid.Add(elements.ElementAt(100));
                }

                centroid.centroid = newCentroid;
            }
        }
    }
}
