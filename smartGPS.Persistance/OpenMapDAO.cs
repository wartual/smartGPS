using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class OpenMapDAO : BaseClass
    {

        public static double TOLERANCE = 0.005;

        public static Node Node_getById(int nodeId)
        {
            return db.Node.Where(item => item.NodeId == nodeId).SingleOrDefault();
        }

        public static IEnumerable<Node> Node_getClosestNode(double latitude, double longitude)
        {
            return db.Node.Where(item => item.NodeLat <= (latitude + TOLERANCE) && item.NodeLat >= (latitude - TOLERANCE) &&
                                    item.NodeLong <= (longitude + (TOLERANCE * 90 / latitude)) && item.NodeLong >= (longitude - (TOLERANCE * 90 / latitude)));
        }
        
        public static IEnumerable<Edge> Edge_getFirstNeighborEdge(int startId, int endId)
        {
            return db.Edge.Include("Way").Include("Way.WayType").Where(item => item.StartId == startId && item.EndId != endId);
        }

        public static IEnumerable<Edge> Edge_getSecondNeghborEdge(int startId, int endId)
        {
            return db.Edge.Include("Way").Include("Way.WayType").Where(item => item.EndId == endId && item.StartId != startId && item.Way.WayOneWay == 0);
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