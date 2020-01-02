using System;
using System.Collections.Generic;

namespace gang
{
    public class Vertex
    {
        public int Target { get; internal set; }

        public Vertex? Next{ get; internal set; }
    }

    public class AdjacencyList
    {
        public Vertex[] Vertices { get; private set; }

        public AdjacencyList(Tuple<int, int> path)
        {
            Vertices = new Vertex[0];

        }
    }
}