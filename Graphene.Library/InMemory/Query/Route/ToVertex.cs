using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.InMemory.Utility;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class ToVertex : IToVertex
    {
        internal ToVertex(WithMinimalEdges withMinimalEdges, int vertexId)
        {
            WithMinimalEdges = withMinimalEdges;
            VertexId = vertexId;
        }
        
        private WithMinimalEdges WithMinimalEdges { get; }
        
        private int VertexId { get; }
        
        public IRouteResult Resolve()
        {
            var graph = WithMinimalEdges.FromVertex.Root.Graph;
            var origin = graph.Vertices.Get(WithMinimalEdges.FromVertex.VertexId);
            var edgeFilter = WithMinimalEdges.Filter;
            var queue = new PriorityQueue<int, Node>();
            queue.Insert(0, new Node {Vertex = origin});
            var visitedNodes = new HashSet<int>();

            while (!queue.IsEmpty)
            {
                var currentNode = queue.RemoveMin();

                if (currentNode.Vertex.Id == VertexId)
                    return PackageResult(currentNode);
                
                if (visitedNodes.Contains(currentNode.Vertex.Id))
                    continue;

                visitedNodes.Add(currentNode.Vertex.Id);

                var edges = currentNode.Vertex.OutgoingEdges
                    .Concat(currentNode.Vertex.BidirectionalEdges);

                if (edgeFilter != null)
                    edges = edges.Where(edgeFilter);
                
                foreach (var edge in edges)
                {
                    var otherVertex = edge.FromVertex.Id == currentNode.Vertex.Id
                        ? edge.ToVertex
                        : edge.FromVertex;
                    
                    if (visitedNodes.Contains(otherVertex.Id))
                        continue;

                    var newNode = new Node
                    {
                        Vertex = otherVertex,
                        Distance = currentNode.Distance + 1,
                        Previous = currentNode,
                        PreviousEdge = edge
                    };
                    
                    queue.Insert(newNode.Distance, newNode);
                }
            }
            
            return new RouteResult(false, origin, Array.Empty<RouteStep>());
        }

        private static IRouteResult PackageResult(Node node)
        {
            var stepStack = new Stack<RouteStep>();
            var currentNode = node;

            while (currentNode.Previous != null)
            {
                stepStack.Push(new RouteStep(currentNode.PreviousEdge, currentNode.Vertex));
                currentNode = currentNode.Previous;
            }
            
            return new RouteResult(true, currentNode.Vertex, stepStack.ToArray());
        }

        private class Node
        {
            public IReadOnlyVertex Vertex { get; set; }
            
            public int Distance { get; set; }
            
            public Node Previous { get; set; }
            
            public IReadOnlyEdge PreviousEdge { get; set; }
        }
    }
}