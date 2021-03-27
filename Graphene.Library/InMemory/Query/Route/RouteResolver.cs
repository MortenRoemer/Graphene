using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.InMemory.Utility;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class RouteResolver<TMetric> where TMetric : IComparable<TMetric>
    {
        private readonly PriorityQueue<TMetric, Node> _queue = new();

        private readonly HashSet<int> _visitedNodes = new();

        private readonly Stack<RouteStep> _stepStack = new();
        
        public RouteResult<TMetric> SolveForMinimalMetric(
            IReadOnlyVertex origin,
            IReadOnlyVertex target,
            Func<IReadOnlyEdge, bool> edgeFilter,
            Func<IReadOnlyEdge, TMetric> metricFunction,
            Func<IReadOnlyVertex, IReadOnlyVertex, TMetric> heuristicFunction,
            Func<TMetric, TMetric, TMetric> accumulatorFunction
        )
        {
            _queue.Clear();
            _queue.Insert(default, new Node {Vertex = origin});
            _visitedNodes.Clear();

            while (!_queue.IsEmpty)
            {
                var currentNode = _queue.RemoveMin();

                if (currentNode.Vertex.Id == target.Id)
                    return PackageResult(currentNode);
                
                if (_visitedNodes.Contains(currentNode.Vertex.Id))
                    continue;

                _visitedNodes.Add(currentNode.Vertex.Id);

                var edges = currentNode.Vertex.OutgoingEdges
                    .Concat(currentNode.Vertex.BidirectionalEdges);

                if (edgeFilter != null)
                    edges = edges.Where(edgeFilter);
                
                foreach (var edge in edges)
                {
                    var otherVertex = edge.FromVertex.Id == currentNode.Vertex.Id
                        ? edge.ToVertex
                        : edge.FromVertex;
                    
                    if (_visitedNodes.Contains(otherVertex.Id))
                        continue;

                    var newNode = new Node
                    {
                        Vertex = otherVertex,
                        Metric = accumulatorFunction(currentNode.Metric, metricFunction(edge)),
                        Previous = currentNode,
                        PreviousEdge = edge
                    };
                    
                    _queue.Insert(accumulatorFunction(newNode.Metric, heuristicFunction(newNode.Vertex, target)), newNode);
                }
            }
            
            return new RouteResult<TMetric>(false, origin, Array.Empty<RouteStep>(), default);
        }

        private RouteResult<TMetric> PackageResult(Node node)
        {
            _stepStack.Clear();
            var currentNode = node;

            while (currentNode.Previous != null)
            {
                _stepStack.Push(new RouteStep(currentNode.PreviousEdge, currentNode.Vertex));
                currentNode = currentNode.Previous;
            }
            
            return new RouteResult<TMetric>(true, currentNode.Vertex, _stepStack.ToArray(), node.Metric);
        }

        private class Node
        {
            public IReadOnlyVertex Vertex { get; set; }
            
            public TMetric Metric { get; set; }
            
            public Node Previous { get; set; }
            
            public IReadOnlyEdge PreviousEdge { get; set; }
        }
    }
}