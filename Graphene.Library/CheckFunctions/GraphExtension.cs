using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Graphene.CheckFunctions
{
    public static class GraphExtension
    {
        public enum EdgeType {Directed, Undirected, Hybrid};

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
            return !FloodFill(graph, graph.Vertices.ElementAt(0)).Values.Any(b => b == false);
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
        public static Dictionary<IVertex,bool> FloodFill(this IGraph graph, IVertex startingVertex)
        {
            if (!graph.Vertices.Contains(startingVertex)) throw new ArgumentException("starting Vertex in not part of the Graph");
            Dictionary<IVertex, bool> vertexCheckList = graph.Vertices.ToDictionary(v => v, v => false);
            var vertexQueue = new Queue<IVertex>();
            vertexQueue.Enqueue(startingVertex);
            vertexCheckList[startingVertex] = true;
            while(vertexQueue.Count > 0)
            {
                var temp = vertexQueue.Dequeue();
                foreach(var vertex in temp.Edges.Select(e => temp.Id == e.FromVertex.Id ? e.ToVertex : e.FromVertex))
                {
                    vertexCheckList[vertex] = true;
                }               
            }
            return vertexCheckList;
        }

        public static List<IEdge> BreadthFirstSearch(this IGraph graph, IVertex startingVertex, IVertex destinationVertex, EdgeType edgeType)
        {
            if (!graph.Vertices.Contains(startingVertex)) throw new ArgumentException("starting Vertex in not part of the Graph");
            if (!graph.Vertices.Contains(destinationVertex)) throw new ArgumentException("destination Vertex is not part of the Graph");
            var vertexCheckList = graph.Vertices.ToDictionary(v => v, v => false);
            var vertexQueue = new Queue<IVertex>();
            var edgesVisited = new List<IEdge>();
            vertexCheckList[startingVertex] = true;
            vertexQueue.Enqueue(startingVertex);
            int i = 0;
            bool destinationFound = false;
            while (vertexQueue.Count > 0 && !destinationFound)
            {
                var temp = vertexQueue.Dequeue();
                foreach (var edge in temp.Edges.Where
                    (e=>((edgeType != EdgeType.Directed && e.Directed == false) || 
                    (edgeType != EdgeType.Undirected && e.Directed == true && e.FromVertex == temp))))
                {
                    var vertex = temp.Id == edge.FromVertex.Id ? edge.ToVertex : edge.FromVertex;
                    if (!vertexCheckList[vertex])
                    {
                        vertexCheckList[vertex] = true;
                        edgesVisited.Add(edge);
                        if (vertex == destinationVertex)
                        {
                            destinationFound = true;
                            break;
                        }
                    }                    
                }
                i++;
            }
            if (destinationFound == false) return null;
            else
            {
                var vertexPath = new List<IEdge>();
                var searchedVertex = destinationVertex;
                while(searchedVertex != startingVertex)
                {
                    var edgeOfPath = edgesVisited.First(e => e.FromVertex == searchedVertex || e.ToVertex == searchedVertex);
                    vertexPath.Add(edgeOfPath);
                    searchedVertex = edgeOfPath.ToVertex == searchedVertex ? edgeOfPath.FromVertex : edgeOfPath.ToVertex;
                }
                vertexPath.Reverse();
                return vertexPath;
            }            
        }
        #endregion



    }
}
