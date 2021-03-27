using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Graphene.Random
{
    public static class RandomGraphs
    {
        private static readonly ThreadLocal<System.Random> Randomizer =
            new ThreadLocal<System.Random>(() => new System.Random(), false);
        
        public static void RandomizeGraph(int vertexCount, double edgeProbability, IGraph graph, EdgeGenerationRule edgeGenerationRule)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException();
            
            graph.Clear();
            var randomizer = Randomizer.Value;

            for(var vertexIndex = 0; vertexIndex < vertexCount; vertexIndex++)
            {
                var newVertex = graph.Vertices.Create();
                IEnumerable<IVertex> vertices = graph.Vertices;

                if (!HasRule(EdgeGenerationRule.AllowEdgesToItself, edgeGenerationRule))
                    vertices = vertices.Where(vertex => vertex.Id != newVertex.Id);
                
                foreach(var vertex in vertices)
                {
                    if (HasRule(EdgeGenerationRule.AllowUndirected, edgeGenerationRule) &&
                        randomizer.NextDouble() < edgeProbability)
                        vertex.BidirectionalEdges.Add(newVertex, null);

                    if (HasRule(EdgeGenerationRule.AllowDirected, edgeGenerationRule))
                    {
                        if (randomizer.NextDouble() < edgeProbability)
                            vertex.IngoingEdges.Add(newVertex, null);

                        if (vertex.Id != newVertex.Id && randomizer.NextDouble() < edgeProbability)
                            vertex.OutgoingEdges.Add(newVertex, null);
                    }
                }
            }
        }

        private static bool HasRule(EdgeGenerationRule expected, EdgeGenerationRule ruleSet)
        {
            return (ruleSet & expected) > 0;
        }
    }
}
