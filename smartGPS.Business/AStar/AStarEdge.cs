using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Persistance;

namespace smartGPS.Business.AStar
{
    public class AStarEdge
    {
        //Edge id
        public int id { get; set; }
        //Start node id
        public int start { get; set; }
        //End node id
        public int end { get; set; }
        //Lenght
        public double length { get; set; }
        //Heading of this edge
        public int heading { get; set; }
        //Way type of this edge
        public int type { get; set; }
        //Speed limit on this edge
        public int speedLimit { get; set; }
        //Obstacle on this edge if any
        public String obstacle { get; set; }

        // constructor
        public AStarEdge()
        {
            this.id = -1;
            this.start = -1;
            this.end = -1;
            this.length = 0;
            this.heading = 0;
            this.type = 0;
            this.speedLimit = 0;
            this.obstacle = "";
        }

        //Construcstor that sets all the parameters 
        public AStarEdge(int id, int start, int end, double length, int heading, int type, int speedLimit, String obstacle) 
        {
            this.id = id;
            this.start = start;
            this.end = end;
            this.length = length;
            this.heading = heading;
            this.type = type;
            this.speedLimit = speedLimit;
            this.obstacle = obstacle;
       }

        public AStarNode getEndNode()
        {
            Node node = OpenMapDAO.Node_getById(end);
            if(node != null)
            {
                AStarNode aStarNode = new AStarNode(node.NodeId, node.NodeLat, node.NodeLong);
                return aStarNode;
            }
            else
            {
                return null; 
            }
        }

        public AStarNode getStartNode()
        {
            Node node = OpenMapDAO.Node_getById(start);
            if (node != null)
            {
                AStarNode aEndNode = new AStarNode(node.NodeId, node.NodeLat, node.NodeLong);
                return aEndNode;
            }
            else
                return null;
        }
    }
}