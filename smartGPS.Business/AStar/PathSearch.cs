﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Business.Custom;
using smartGPS.Business.ExternalServices;
using smartGPS.Business.Models.OpetWeather;
using smartGPS.Business.Models.PrometInfo;
using smartGPS.Persistance;

namespace smartGPS.Business.AStar
{
    public class PathSearch
    {
        public static int WALKING = 0;
        public static int DRIVING = 1;

        private double rainCoefficient { get; set; }
        private double trafficCoefficinet { get; set; }
        private double trafficRadius { get; set; }
        private Haversine Haversine { get; set; }
        private List<WeatherResponse> weatherPoints { get; set; }
        private Dictionary<long, Event> trafficEvents { get; set; }
        private Dictionary<int, AStarNode> openSet { get; set; }
        private Dictionary<int, AStarNode> closedSet{get; set; }
        private PriorityQueue queue { get; set; }

        public PathSearch()
        {
            this.Haversine = new Haversine();
            this.weatherPoints = new List<WeatherResponse>();
            this.rainCoefficient = Config.RAIN_COEFFICINET;
            this.trafficCoefficinet = Config.TRAFFIC_COEFFICINET;
            this.trafficRadius = Config.TRAFFIC_RADIUS;
        }

        /**
         * Gets weather data on points along the direct line from start to finish. Points are positions on every
         *  5 kilometers and are later used in classification. This reduces calls to weather service and speeds up
         *  * path search
         */
        private void prepareWeatherData(double startLat, double startLong, double endLat, double endLong)
        {
            WeatherResponse response;
            double distance = Haversine.Distance(startLat, startLong, endLat, endLong, Custom.Haversine.DistanceType.Kilometers);
            double currentLat = startLat;
            double currentLong = startLong;
            int weatherPointsNum = (int)Math.Ceiling(distance);

            double vectorLat = (endLat - startLat) / weatherPointsNum;
            double vectorLong = (endLong - startLong) / weatherPointsNum;
            response = OpenWeatherManagement.getWeatherByGPS(startLat, startLong);
            weatherPoints.Add(response);

            for (int i = 0; i < weatherPointsNum; i++)
            {
                currentLat = currentLat + vectorLat;
                currentLong = currentLong + vectorLong;
                response = OpenWeatherManagement.getWeatherByGPS(currentLat, currentLong);
                weatherPoints.Add(response);
            }
        }

        private void prepareTrafficData(double startLat, double startLong, double endLat, double endLong)
        {
            double distance = Haversine.Distance(startLat, startLong, endLat, endLong, Custom.Haversine.DistanceType.Kilometers);
            double currentLat = startLat;
            double currentLong = startLong;
            int trafficPointsNum = (int)Math.Ceiling(distance);

            double vectorLat = (endLat - startLat) / trafficPointsNum;
            double vectorLong = (endLong - startLong) / trafficPointsNum;
            double tmpDistance;

            trafficEvents = new Dictionary<long, Event>();

            PrometInfoResponse response = PrometInfoManagement.getInfo();
            IEnumerable<Event> activeEvents = response.Events.Event.Where(item => item.VeljavnostDoDateTime >= DateTime.Now);

            foreach(Event trafficEvent in activeEvents)
            {
                 tmpDistance = Haversine.Distance(startLat, startLong, trafficEvent.Y_WGS, trafficEvent.X_WGS, Custom.Haversine.DistanceType.Kilometers);
                 if (tmpDistance < trafficRadius && !trafficEvents.ContainsKey(trafficEvent.Id))
                        trafficEvents.Add(trafficEvent.Id, trafficEvent);
            }

            for (int i = 0; i < trafficPointsNum; i++)
            {
                 currentLat = currentLat + vectorLat;
                 currentLong = currentLong + vectorLong;

                foreach(Event trafficEvent in activeEvents)
                {
                    tmpDistance = Haversine.Distance(currentLat, currentLong, trafficEvent.Y_WGS, trafficEvent.X_WGS, Custom.Haversine.DistanceType.Kilometers);
                    if (tmpDistance < trafficRadius && !trafficEvents.ContainsKey(trafficEvent.Id))
                        trafficEvents.Add(trafficEvent.Id, trafficEvent);
                }
            }
        }

        private Event getClosestTrafficEvent(double latitude, double longitude)
        {
            int minDistanceIndex = 0;
            double distance = Double.MaxValue;
            double tempDistance = 0;
            KeyValuePair<long, Event> trafficEvent = new KeyValuePair<long,Event>();
      
            foreach(KeyValuePair<long, Event> tmpEvent in trafficEvents)
            {
                tempDistance = Math.Sqrt(Math.Pow((latitude - tmpEvent.Value.Y_WGS), 2) + Math.Pow((longitude - tmpEvent.Value.X_WGS), 2));
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    trafficEvent = tmpEvent;
                }
            }

            if(trafficEvent.Value != null && Haversine.Distance(latitude, longitude, trafficEvent.Value.Y_WGS, trafficEvent.Value.X_WGS, Custom.Haversine.DistanceType.Kilometers) <= trafficRadius)
            {
                return trafficEvent.Value;
            }
            else
            {
                return null;
            }
        }

        // index of weather point in weather points list that is closest to observed location
        private int getClosestWeatherPoint(double latitude, double longitude){
            int minDistanceIndex = 0;
            double distance = Double.MaxValue;
            double tempDistance = 0;

            for (int i = 0; i < weatherPoints.Count; i++)
            {
	            tempDistance = Math.Sqrt(Math.Pow((latitude - weatherPoints.ElementAt(i).Coordinates.Latitude), 2) + Math.Pow((longitude - weatherPoints.ElementAt(i).Coordinates.Longitude), 2));
                if(tempDistance < distance){
                    distance = tempDistance;
                    minDistanceIndex = i;
                }
            }

            return minDistanceIndex;
        }

        // Used to find start and target node according to provided latitude and longitude
        private AStarNode closestAStarNode(double latitude, double longitude)
        {
            int id = -1;
            double minDistance = 100000000; //100000 km which is more than equator
            double distance = 0;
            double latitudeMap;
            double longitudeMap;
            AStarNode returnNode = new AStarNode();

            IEnumerable<Node> nodes = OpenMapDAO.Node_getClosestNode(latitude, longitude);

            foreach (Node node in nodes)
            {
                latitudeMap = node.NodeLat;
                longitudeMap = node.NodeLong;
                distance = Haversine.Distance(latitude, longitude, latitudeMap, longitudeMap, Custom.Haversine.DistanceType.Kilometers);

                if (distance < minDistance)
                {
                    id = node.NodeId;
                    minDistance = distance;
                    returnNode.id = id;
                    returnNode.latitude = latitudeMap;
                    returnNode.longitude = longitudeMap;
                }
            }

            return returnNode;
        }

        // Calculates best route using user data and current data. It uses A star alorithm that allows loops due to possible route that must go to
        // the same node twice. This poses danger of long calculation time in certain cases.
        public List<SmartNode> search(double startLat, double startLong, double targetLat, double targetLong, int mode, int div)
        {
            List<SmartNode> returnList = new List<SmartNode>();
            int tempTime = 0;
            int weatherIndex = 0;
            int trafficIndex;

            openSet = new Dictionary<int, AStarNode>();
            closedSet = new Dictionary<int, AStarNode>();
            queue = new PriorityQueue();
            AStarNode goal = null;
            AStarNode startNode = closestAStarNode(startLat, startLong);
            AStarNode target = closestAStarNode(targetLat, targetLong);
            AStarNode nodeForEarlyExploration = null;
            WeatherResponse weather;
            Event roadEvent;
            double g;
            Boolean containsKey;
                      
            prepareWeatherData(startLat, startLong, targetLat, targetLong);
            prepareTrafficData(startLat, startLong, targetLat, targetLong);
            startNode.h = Haversine.Distance(startNode.latitude, startNode.longitude, target.latitude, target.longitude, Custom.Haversine.DistanceType.Kilometers);

            openSet.Add(startNode.id, startNode);
            queue.add(startNode);

            int howMuch = 0;
            AStarNode x;

            while (openSet.Count > 0)
            {
                howMuch++;
                x = queue.poll();
                openSet.Remove(x.id);
                double tmp_distance = Haversine.Distance(x.latitude, x.longitude, target.latitude, target.longitude, Custom.Haversine.DistanceType.Kilometers);

                if (x.id == target.id)
                {
                    goal = x;
                    break;
                }
                else
                {
                    if (closedSet.ContainsKey(x.id))
                        closedSet[x.id] = x;
                    else
                        closedSet.Add(x.id, x);
                    x.getNeighbourEdges(mode);

                    weatherIndex = getClosestWeatherPoint(x.latitude, x.longitude);
                    weather = weatherPoints.ElementAt(weatherIndex);

                    roadEvent = getClosestTrafficEvent(x.latitude, x.longitude);

                     foreach (AStarEdge neighbor in x.outEdges) 
                     {
                         // calculate cost
                         g = x.g + neighbor.length / 1000 + Config.getWeatherCoefficient(weather) + Config.getRoadEventCoefficient(roadEvent);
                         AStarNode n = null;

                         if (openSet.ContainsKey(neighbor.getEndNode().id))
                         {
                             n = openSet[neighbor.getEndNode().id];
                         }
                         
                         //check if node n is in open set, if it is not add it to the open set
                         if (n == null)
                         {
                             n = new AStarNode(neighbor.getEndNode().id, neighbor.getEndNode().latitude, neighbor.getEndNode().longitude);
                             n.h = Haversine.Distance(n.latitude, n.longitude, target.latitude, target.longitude, Custom.Haversine.DistanceType.Kilometers) * div;
                             n.cameFrom = x;
                             n.g = g;
                             openSet.Add(n.id, n);
                             queue.add(n);
                         }
                         else if (g < n.g)
                         {
                             //Have a better route to the current node, change its parent
                             n.cameFrom = x;
                             n.h = Haversine.Distance(n.latitude, n.longitude, target.latitude, target.longitude, Custom.Haversine.DistanceType.Kilometers) * div;
                             n.g = g;
                         }
                     }
                }
            }

            if (goal != null)
            {
                Stack<AStarNode> stack = new Stack<AStarNode>();
                List<AStarNode> list = new List<AStarNode>();

                stack.Push(goal);
                AStarNode parent = goal.cameFrom;
                g = (long)(goal.g * 1000.0);

                 //create path from getting parent of every node
                 while(parent != null){
                    stack.Push(parent);
                    parent = parent.cameFrom;
                }

                while(stack.Count > 0){
                    list.Add(stack.Pop());
                }

                 foreach(AStarNode n in list)
                {
                    returnList.Add(new SmartNode(n.latitude, n.longitude, g));
                }
            }

            return returnList;
        }

        
    }
}