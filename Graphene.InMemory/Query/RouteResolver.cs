using System;
using System.Collections.Generic;
using Graphene.InMemory.Utility;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query
{
    public readonly struct RouteResolver<TMetric> where TMetric : IComparable<TMetric>
    {
        public RouteResolver(MemoryGraph graph)
        {
            Graph = graph;
        }

        private readonly MemoryGraph Graph { get; }

        public RouteResult<TMetric> SolveForMinimalMetric(
            Guid originId,
            Guid targetId,
            Func<IReadOnlyEdge, bool>? edgeFilter,
            Func<IReadOnlyEdge, TMetric> metricFunction,
            TMetric defaultCost,
            Func<IReadOnlyVertex, IReadOnlyVertex, TMetric> heuristicFunction,
            Func<TMetric, TMetric, TMetric> accumulatorFunction
        )
        {
            Utility.PriorityQueue<TMetric, Node> queue = new();
            HashSet<Guid> visitedNodes = new();
            var origin = Graph._Entities[originId] as IReadOnlyVertex;
            var target = Graph._Entities[targetId] as IReadOnlyVertex;
            queue.Insert(defaultCost, new Node(origin!, defaultCost, null, null));

            while (!queue.IsEmpty)
            {
                var currentNode = queue.RemoveMin();

                if (currentNode.Vertex.Id == targetId)
                    return PackageResult(currentNode);
                
                if (visitedNodes.Contains(currentNode.Vertex.Id))
                    continue;

                visitedNodes.Add(currentNode.Vertex.Id);

                foreach (var edge in Graph.GetOutgoingEdgesForVertex(currentNode.Vertex.Id, edgeFilter))
                {
                    var otherVertexId = edge.FromVertex == currentNode.Vertex.Id
                        ? edge.ToVertex
                        : edge.FromVertex;
                    
                    if (visitedNodes.Contains(otherVertexId))
                        continue;

                    var newNode = new Node(
                        (Graph._Entities[otherVertexId] as IReadOnlyVertex)!, 
                        accumulatorFunction(currentNode.Cost, metricFunction(edge)),
                        currentNode,
                        edge);

                    queue.Insert(accumulatorFunction(newNode.Cost, heuristicFunction.Invoke(newNode.Vertex, target!)), newNode);
                }
            }
            
            return new RouteResult<TMetric>(false, origin!, Array.Empty<RouteStep>(), defaultCost);
        }

        private static RouteResult<TMetric> PackageResult(Node node)
        {
            Stack<RouteStep> stepStack = new();
            var currentNode = node;

            while (currentNode.Previous != null)
            {
                stepStack.Push(new RouteStep(currentNode.PreviousEdge!, currentNode.Vertex));
                currentNode = currentNode.Previous;
            }
            
            return new RouteResult<TMetric>(true, currentNode.Vertex, stepStack.ToArray(), node.Cost);
        }

        private class Node
        {
            public Node(IReadOnlyVertex vertex, TMetric cost, Node? previous, IReadOnlyEdge? previousEdge)
            {
                Vertex = vertex;
                Cost = cost;
                Previous = previous;
                PreviousEdge = previousEdge;
            }
            
            public IReadOnlyVertex Vertex { get; }
            
            public TMetric Cost { get; }
            
            public Node? Previous { get; }
            
            public IReadOnlyEdge? PreviousEdge { get; }
        }
    }
}