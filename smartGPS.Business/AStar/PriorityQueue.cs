using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business.AStar
{
    public class PriorityQueue
    {

        private List<AStarNode> nodes { get; set; }

        public PriorityQueue()
        {
            this.nodes = new List<AStarNode>();
        }

        public bool isEmpty()
        {
            return nodes.Count == 0;
        }

        public int size()
        {
            return nodes.Count();
        }

        public AStarNode get(int position)
        {
            return nodes.ElementAt(position);
        }

        public AStarNode poll()
        {
            AStarNode tmp = nodes.First();
            nodes.RemoveAt(0);
            nodes.Sort();
            return tmp;
        }

        public void add(AStarNode node)
        {
            nodes.Add(node);
            nodes.Sort();
        }

        public void removeAt(int position)
        {
            if (position < size())
            {
                nodes.RemoveAt(position);
            }
        }

        public void remove(AStarNode node)
        {
            nodes.Remove(node);
        }
    }
}