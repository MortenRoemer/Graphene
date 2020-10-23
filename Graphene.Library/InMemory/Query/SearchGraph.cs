using System;
using System.Collections.Generic;
using System.Linq;

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
            IEnumerable<ulong> essentialEntities,
            Func<IEdge, double> weightFunction
        )
        {
            var vertices = new Dictionary<ulong, Vertex>();
            var hasNegativeWeights = false;
            var essentialEntitySet = new SortedSet<ulong>(essentialEntities);

            foreach (
                var vertex in graph.Vertices
                .Where(
                    vertex => 
                    essentialEntitySet.Contains(vertex.Id) ||
                    (routeDefinition.VertexFilter?.Contains(vertex) ?? true)
                )
            )
            {
                vertices.Add(vertex.Id, new Vertex { Origin = vertex });
            }

            foreach (var vertex in vertices.Values)
            {
                var edges = new List<Edge>();
                
                edges.AddRange(
                    vertex.Origin.OutgoingEdges
                    .Where(
                        edge =>
                        vertices.ContainsKey(edge.ToVertex.Id) &&
                        essentialEntitySet.Contains(edge.Id) ||
                        (routeDefinition.EdgeFilter?.Contains(edge) ?? true)
                    )
                    .Select(
                        edge => new Edge {
                            Origin = edge,
                            Weight = weightFunction.Invoke(edge),
                            Target = vertices[edge.ToVertex.Id]
                        }
                    )
                );
            }

            return new SearchGraph(vertices, hasNegativeWeights);
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
    }
}