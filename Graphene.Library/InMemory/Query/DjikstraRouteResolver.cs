using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.InMemory.Utility;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query
{
    public static class DjikstraRouteResolver
    {
        internal static RouteResult<TMetric> SolveForMinimalMetric<TMetric>(
            IReadOnlyVertex origin,
            IReadOnlyVertex target,
            Func<IReadOnlyEdge, bool> edgeFilter, 
            Func<IReadOnlyEdge, TMetric> metricFunction,
            Func<TMetric, TMetric, TMetric> accumulatorFunction
        ) where TMetric : IComparable<TMetric>
        {
            var queue = new PriorityQueue<TMetric, Node<TMetric>>();
            queue.Insert(default, new Node<TMetric> {Vertex = origin});
            var visitedNodes = new HashSet<int>();

            while (!queue.IsEmpty)
            {
                var currentNode = queue.RemoveMin();

                if (currentNode.Vertex.Id == target.Id)
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

                    var newNode = new Node<TMetric>
                    {
                        Vertex = otherVertex,
                        Metric = accumulatorFunction(currentNode.Metric, metricFunction(edge)),
                        Previous = currentNode,
                        PreviousEdge = edge
                    };
                    
                    queue.Insert(newNode.Metric, newNode);
                }
            }
            
            return new RouteResult<TMetric>(false, origin, Array.Empty<RouteStep>(), default);
        }

        private static RouteResult<TMetric> PackageResult<TMetric>(Node<TMetric> node)
        {
            var stepStack = new Stack<RouteStep>();
            var currentNode = node;

            while (currentNode.Previous != null)
            {
                stepStack.Push(new RouteStep(currentNode.PreviousEdge, currentNode.Vertex));
                currentNode = currentNode.Previous;
            }
            
            return new RouteResult<TMetric>(true, currentNode.Vertex, stepStack.ToArray(), node.Metric);
        }

        private class Node<TMetric>
        {
            public IReadOnlyVertex Vertex { get; set; }
            
            public TMetric Metric { get; set; }
            
            public Node<TMetric> Previous { get; set; }
            
            public IReadOnlyEdge PreviousEdge { get; set; }
        }
    }
}