using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.InMemory.Utility;

namespace Graphene.InMemory.Query
{
    internal class SearchGraph
    {
        private SearchGraph(IReadOnlyDictionary<ulong, Vertex> vertices, bool hasNegativeWeights)
        {
            Vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
            HasNegativeWeights = hasNegativeWeights;
        }

        private readonly IReadOnlyDictionary<ulong, Vertex> Vertices;

        private readonly bool HasNegativeWeights;

        public static SearchGraph CreateForRoute(
            IGraph graph,
            BuilderRoute routeDefinition,
            IEnumerable<ulong> essentialVertices,
            Func<IEdge, double> weightFunction
        )
        {
            var vertices = new Dictionary<ulong, Vertex>();
            var hasNegativeWeights = false;
            var essentialVertexSet = new SortedSet<ulong>(essentialVertices);

            foreach (
                var vertex in graph.Vertices
                .Where(
                    vertex => 
                    essentialVertexSet.Contains(vertex.Id) ||
                    (routeDefinition.VertexFilter?.Contains(vertex) ?? true)
                )
            )
            {
                vertices.Add(vertex.Id, new Vertex { Origin = vertex });
            }

            foreach (var vertex in vertices.Values)
            {
                var edges = new List<Edge>();
                
                foreach (var edge in
                    vertex.Origin.OutgoingEdges
                    .Where(
                        edge =>
                        edge.FromVertex.Id != edge.ToVertex.Id &&
                        vertices.ContainsKey(edge.ToVertex.Id) &&
                        (routeDefinition.EdgeFilter?.Contains(edge) ?? true)
                    )
                )
                {
                    var searchEdge = new Edge
                    {
                        Origin = edge,
                        Weight = weightFunction.Invoke(edge),
                        Target = vertices[edge.ToVertex.Id]
                    };

                    hasNegativeWeights = hasNegativeWeights || searchEdge.Weight < 0;
                    edges.Add(searchEdge); 
                }

                foreach (var edge in
                    vertex.Origin.BidirectionalEdges
                    .Where(
                        edge =>
                        edge.FromVertex.Id != edge.ToVertex.Id &&
                        vertices.ContainsKey(GetOtherVertexId(edge, vertex.Origin.Id)) &&
                        (routeDefinition.EdgeFilter?.Contains(edge) ?? true)
                    )
                )
                {
                    var searchEdge = new Edge
                    {
                        Origin = edge,
                        Weight = weightFunction.Invoke(edge),
                        Target = vertices[GetOtherVertexId(edge, vertex.Origin.Id)]
                    };

                    hasNegativeWeights = hasNegativeWeights || searchEdge.Weight < 0;
                    edges.Add(searchEdge); 
                }

                vertex.Edges = edges.ToArray();
            }

            return new SearchGraph(vertices, hasNegativeWeights);
        }

        private static ulong GetOtherVertexId(IEdge edge, ulong origin)
            => edge.FromVertex.Id == origin
                ? edge.ToVertex.Id
                : edge.FromVertex.Id;

        public bool FindMinRoute(ulong fromVertex, ulong toVertex, out IEnumerable<IEntity> result)
        {
            if (HasNegativeWeights)
                throw new NotImplementedException();

            return FindWithDjikstra(fromVertex, toVertex, out result);
        }

        private bool FindWithDjikstra(ulong fromVertex, ulong toVertex, out IEnumerable<IEntity> result)
        {
            var queue = new PriorityQueue<double, DjikstraVertex>();
            var visitedVertices = new HashSet<ulong>();

            if (Vertices.TryGetValue(fromVertex, out var startVertex))
                queue.Insert(0, new DjikstraVertex {Vertex = startVertex});
            else
            {
                result = default;
                return false;
            }

            while (!queue.IsEmpty)
            {
                var currentVertex = queue.RemoveMin();

                if (currentVertex.Vertex.Origin.Id == toVertex)
                {
                    result = GetRouteFromDjikstra(currentVertex);
                    return true;
                }

                if (visitedVertices.Contains(currentVertex.Vertex.Origin.Id))
                    continue;

                visitedVertices.Add(currentVertex.Vertex.Origin.Id);

                foreach (var edge in currentVertex.Vertex.Edges)
                {
                    if (visitedVertices.Contains(edge.Target.Origin.Id))
                        continue;

                    var edgeCost = currentVertex.Cost + edge.Weight;
                    queue.Insert(edgeCost, new DjikstraVertex{Vertex = edge.Target, Cost = edgeCost, PreviousVertex = currentVertex});
                }
            }

            result = default;
            return false;
        }

        private static IEnumerable<IEntity> GetRouteFromDjikstra(DjikstraVertex vertex)
        {
            var result = new List<IEntity>();
            
            while (vertex != null)
            {
                result.Add(vertex.Vertex.Origin);
                vertex = vertex.PreviousVertex;
            }

            result.Reverse();
            return result;
        }

        private class Vertex
        {
            public IVertex Origin;
            public IReadOnlyList<Edge> Edges;
        }

        private struct Edge
        {
            public IEdge Origin;
            public double Weight;
            public Vertex Target;
        }

        private class DjikstraVertex
        {
            public Vertex Vertex;
            public double Cost;
            public DjikstraVertex PreviousVertex;
        }
    }
}