using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Persistance;

namespace smartGPS.Business.AStar
{
    public class AStarNode : IComparable<AStarNode>
    {
        //Node id
        public int id { get; set; }

        //Node latitude
        public double latitude { get; set; }

        //Node longitude
        public double longitude { get; set; }

        //Node id of predecessor node along path
        public AStarNode cameFrom { get; set; }

        //g value (path cost from start to this node)
        public double g { get; set; }

        //h value (estimated cost to target node - air distance in this case)
        public double h { get; set; }

        //list of edges that start or end in this node
        public List<AStarEdge> outEdges { get; set; }

        // f value of this node for A star algorithm
        public double f
        {
            get
            {
                return g + h;
            }
        }

        // Empty constructor
        public AStarNode(){
            this.outEdges = new List<AStarEdge>();
        }

        // Constructor that sets latitude, longitde and node id
        public AStarNode(int id, double latitude, double longitude) {
            this.outEdges = new List<AStarEdge>();
            this.id = id;
            this.latitude = latitude;
            this.longitude = longitude; 
        }

        public void getNeighbourEdges()
        {
            Edge tmp;
            String obstacleType = "";
            short obstacleNoPass = 0;
            Obstacle obstacle;
            int wayMaxSpeed = 0;
            
            IEnumerable<Edge> edges = OpenMapDAO.Edge_getFirstNeighborEdge(id, id);
            
            foreach (Edge edge in edges)
            {
                tmp = OpenMapDAO.Edge_getById(edge.EdgeId);
                obstacle = OpenMapDAO.Obstacle_getByEdgeId(tmp.EdgeId);

                if (obstacle != null)
                {
                    obstacleType = obstacle.ObstacleType;
                    obstacleNoPass = obstacle.ObstacleNoPass;
                }

                if (obstacleNoPass == 1)
                {
                    continue;
                }

                if (tmp.Way.WayMaxSpeed == 0)
                {
                    wayMaxSpeed = tmp.Way.WayType.WayTypeDefSpeed; 
                }
                else
                {
                    wayMaxSpeed = tmp.Way.WayMaxSpeed;
                }

                outEdges.Add(new AStarEdge(tmp.EdgeId, id, tmp.EndId, tmp.EdgeLength, Convert.ToInt32(tmp.EdgeHeading), tmp.Way.WayType.WayTypeName, wayMaxSpeed, obstacleType));
            }

            edges = OpenMapDAO.Edge_getSecondNeghborEdge(id, id);

            foreach(Edge edge in edges)
            {
                tmp = OpenMapDAO.Edge_getById(edge.EdgeId);
                obstacle = OpenMapDAO.Obstacle_getByEdgeId(tmp.EdgeId);

                if (obstacle != null)
                {
                    obstacleType = obstacle.ObstacleType;
                    obstacleNoPass = obstacle.ObstacleNoPass;
                }

                if (obstacleNoPass == 1)
                {
                    continue;
                }

                if (tmp.Way.WayMaxSpeed == 0)
                {
                    wayMaxSpeed = tmp.Way.WayType.WayTypeDefSpeed;
                }
                else
                {
                    wayMaxSpeed = tmp.Way.WayMaxSpeed;
                }

                outEdges.Add(new AStarEdge(tmp.EdgeId, id, edge.StartId, edge.EdgeLength, Convert.ToInt32(tmp.EdgeHeading), tmp.Way.WayType.WayTypeName, wayMaxSpeed, obstacleType));
            }

            return;
        }

        public int CompareTo(AStarNode other)
        {
            if (this.f < other.f)
            {
                return -1;
            }
            else if (this.f > other.f)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }     
}