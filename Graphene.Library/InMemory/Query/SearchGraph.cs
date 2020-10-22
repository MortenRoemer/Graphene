using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory.Query
{
    internal class SearchGraph
    {
        private SearchGraph(Vertex[] vertices, bool hasNegativeWeights)
        {
            Vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
            HasNegativeWeights = hasNegativeWeights;
        }

        private readonly Vertex[] Vertices;

        private readonly bool HasNegativeWeights;

        public static SearchGraph CreateForRoute(IGraph graph, IEntity origin, BuilderRoute routeDefinition, double defaultWeight)
        {
            throw new NotImplementedException();
        }

        private class Vertex
        {
            public readonly IVertex Origin;
            public readonly Edge[] Edges;
        }

        private struct Edge
        {
            public readonly IEdge Origin;
            public readonly double Weight;
            public readonly Vertex Target;
        }
    }
}