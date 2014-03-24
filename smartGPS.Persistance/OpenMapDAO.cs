using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class OpenMapDAO : BaseClass
    {

        public static double TOLERANCE = 0.005;
        public static IEnumerable<Node> nodes;
        public static IEnumerable<Edge> edges;
        public static IEnumerable<Obstacle> obstacles;

        public static List<int> WALKING_OPTIONS = new List<int>() { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        public static List<int> DRIVING_OPTIONS = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14, 15, 16 };


        private static void cacheNodes()
        {
            nodes = db.Node;
        }

        private static void cacheEdges()
        {
            edges = db.Edge.Include("Way").Include("Way.WayType");
        }

        private static void cacheObstacles()
        {
            obstacles = db.Obstacle;
        }

        public static void clearCache()
        {
            edges = null;
            nodes = null;
        }

        public static Node Node_getById(int nodeId)
        {
            return db.Node.Where(item => item.NodeId == nodeId).SingleOrDefault();
        }

        public static IEnumerable<Node> Node_getClosestNode(double latitude, double longitude)
        {
            return db.Node.Where(item => item.NodeLat <= (latitude + TOLERANCE) && item.NodeLat >= (latitude - TOLERANCE) &&
                                    item.NodeLong <= (longitude + (TOLERANCE * 90 / latitude)) && item.NodeLong >= (longitude - (TOLERANCE * 90 / latitude)));
        }
        
        public static IEnumerable<Edge> Edge_getFirstNeighborEdge(int startId, int endId, int? travelTypeId)
        {
            if (travelTypeId.HasValue)
            {
                if (travelTypeId.Value == 0)
                {
                    return db.Edge.Include("Way").Include("Way.WayType").Where(item => item.StartId == startId && item.EndId != endId && WALKING_OPTIONS.Contains(item.Way.WayTypeId));
                }
                else
                {
                    return db.Edge.Include("Way").Include("Way.WayType").Where(item => item.StartId == startId && item.EndId != endId && DRIVING_OPTIONS.Contains(item.Way.WayTypeId));
                }
               
            }
            else
            {
                return db.Edge.Include("Way").Include("Way.WayType").Where(item => item.StartId == startId && item.EndId != endId);
            }
        }

        public static IEnumerable<Edge> Edge_getSecondNeghborEdge(int startId, int endId, int? travelTypeId)
        {
            if (travelTypeId.HasValue)
            {
                if (travelTypeId.Value == 0)
                {
                    return db.Edge.Include("Way").Include("Way.WayType").Where(item => item.EndId == endId && item.StartId != startId && item.Way.WayOneWay == 0 && WALKING_OPTIONS.Contains(item.Way.WayTypeId));
                }
                else
                {
                    return db.Edge.Include("Way").Include("Way.WayType").Where(item => item.EndId == endId && item.StartId != startId && item.Way.WayOneWay == 0 && DRIVING_OPTIONS.Contains(item.Way.WayTypeId));
                }
            }
            else
            {
                return db.Edge.Include("Way").Include("Way.WayType").Where(item => item.EndId == endId && item.StartId != startId && item.Way.WayOneWay == 0);
            }
        }

        public static Edge Edge_getById(int id)
        {
           
            return db.Edge.Where(item => item.EdgeId == id).SingleOrDefault();
        }

        public static Obstacle Obstacle_getByEdgeId(int edgeId)
        {
            return db.Obstacle.Where(item => item.EdgeId == edgeId).SingleOrDefault();
        }
    }
}