using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Graphene.CheckFunctions
{
    public static class GraphExtension
    {
        #region Boolean Checks
        public static bool IsBipartite(this IGraph graph)
        {
            //check if maxcut = edge.count
            throw new NotImplementedException();
        }

        public static bool IsChordal(this IGraph graph)
        {
            throw new NotImplementedException();
        }

        public static bool IsConnected(this IGraph graph)
        {
            if (graph.Size == 0) return true;
            return BreadthFirstSearch(graph, graph.Vertices.ElementAt(0)) == graph.Vertices;
        }

        public static bool IsPerfect(this IGraph graph)
        {
            //https://algorithms.leeds.ac.uk/wp-content/uploads/sites/117/2017/09/FOCS03final.pdf
            throw new NotImplementedException();
        }

        public static bool IsPlanar(this IGraph graph)
        {
            //http://jgaa.info/accepted/2004/BoyerMyrvold2004.8.3.pdf
            throw new NotImplementedException();
        }

        public static bool IsTrianglefree(this IGraph graph)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Graph Algorithms
        public static List<IVertex> BreadthFirstSearch(this IGraph graph, IVertex startingVertex)
        {
            var VerticesVisited = new List<IVertex>();
            VerticesVisited.Add(startingVertex);
            int i = 0;
            while(i < VerticesVisited.Count)
            {
                //Todo: Update once allEdges Feature of Nodes is implemented
                foreach(var vertex in VerticesVisited[i].BidirectionalEdges.Select(e=> VerticesVisited[i].Id == e.FromVertex.Id ? e.ToVertex : e.FromVertex))
                {
                    if (!VerticesVisited.Contains(vertex)) VerticesVisited.Add(vertex);
                }
                foreach(var vertex in VerticesVisited[i].IngoingEdges.Select(e => e.FromVertex))
                {
                    if (!VerticesVisited.Contains(vertex)) VerticesVisited.Add(vertex);
                }
                foreach (var vertex in VerticesVisited[i].OutgoingEdges.Select(e => e.ToVertex))
                {
                    if (!VerticesVisited.Contains(vertex)) VerticesVisited.Add(vertex);
                }
                i++;
            }
            return VerticesVisited;
        }
        #endregion



    }
}
