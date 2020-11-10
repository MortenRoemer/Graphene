using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Graphene.Random
{
    public static class RandomGraphs
    {
        public enum EdgeType : byte {BidirectionalOnly, DirectedOnly, Hybrid};
        
        private static readonly ThreadLocal<System.Random> Randomizer =
            new ThreadLocal<System.Random>(() => new System.Random(), false);
        
        public static IGraph GetRandomGraph(int vertexCount, double edgeProbability, Func<IGraph> graphConstructor, EdgeType edgeType, bool allowSelfDirectionalEdges)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException();
            
            var graph = graphConstructor.Invoke();
            var randomizer = Randomizer.Value;

            for(var vertexIndex = 0; vertexIndex < vertexCount; vertexIndex++)
            {
                var newVertex = graph.Vertices.Create();
                IEnumerable<IVertex> vertices = graph.Vertices;

                if (!allowSelfDirectionalEdges)
                    vertices = vertices.Where(vertex => vertex.Id != newVertex.Id);
                
                foreach(var vertex in vertices)
                {
                    switch (edgeType)
                    {
                        case EdgeType.BidirectionalOnly:
                            if (randomizer.NextDouble() < edgeProbability)
                                vertex.BidirectionalEdges.Add(newVertex);
                            break;
                        case EdgeType.DirectedOnly:
                            if (randomizer.NextDouble() < edgeProbability)
                                vertex.IngoingEdges.Add(newVertex);
                            if (vertex.Id != newVertex.Id && randomizer.NextDouble() < edgeProbability)
                                vertex.OutgoingEdges.Add(newVertex);
                            break;
                        case EdgeType.Hybrid:
                            if (randomizer.NextDouble() < edgeProbability)
                                vertex.IngoingEdges.Add(newVertex);
                            if (vertex.Id != newVertex.Id && randomizer.NextDouble() < edgeProbability)
                                vertex.OutgoingEdges.Add(newVertex);
                            if (randomizer.NextDouble() < edgeProbability)
                                vertex.BidirectionalEdges.Add(newVertex);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(edgeType), edgeType, null);
                    }                                        
                }
            }
            return graph;
        }
    }
}
