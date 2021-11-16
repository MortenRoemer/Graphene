using System.Collections.Generic;
using System.Threading;
using Graphene.Transactions;

namespace Graphene.Random
{
    public static class RandomGraphs
    {
        private static readonly ThreadLocal<System.Random> Randomizer =
            new(() => new System.Random(), false);
        
        public static async void RandomizeGraph(IGraph graph, int vertexCount, string vertexLabel, double edgeProbability, string edgeLabel, EdgeGenerationRule edgeGenerationRule)
        {
            var randomizer = Randomizer.Value;
            var vertices = new List<IVertex>();
            var transaction = new Transaction();

            for(var i = 0; i < vertexCount; i++)
            {
                var newVertex = new Vertex(vertexLabel);
                vertices.Add(newVertex);
                transaction.Add(newVertex.ToCreateVertexAction());

                var bound = HasRule(EdgeGenerationRule.AllowEdgesToItself, edgeGenerationRule)
                    ? vertices.Count
                    : vertices.Count - 1;

                for(int vertexIndex = 0; vertexIndex < bound; vertexIndex++)
                {
                    var vertex = vertices[vertexIndex];

                    if (HasRule(EdgeGenerationRule.AllowUndirected, edgeGenerationRule) &&
                        randomizer!.NextDouble() < edgeProbability)
                    {
                        var edge = new Edge(edgeLabel, vertex.Id, newVertex.Id, false);
                        transaction.Add(edge.ToCreateEdgeAction());
                    }

                    if (HasRule(EdgeGenerationRule.AllowDirected, edgeGenerationRule))
                    {
                        if (randomizer!.NextDouble() < edgeProbability)
                        {
                            var edge = new Edge(edgeLabel, vertex.Id, newVertex.Id, true);
                            transaction.Add(edge.ToCreateEdgeAction());
                        }

                        if (vertex.Id != newVertex.Id && randomizer.NextDouble() < edgeProbability)
                        {
                            var edge = new Edge(edgeLabel, newVertex.Id, vertex.Id, true);
                            transaction.Add(edge.ToCreateEdgeAction());
                        }
                    }
                }
            }

            await graph.Execute(transaction);
        }

        private static bool HasRule(EdgeGenerationRule expected, EdgeGenerationRule ruleSet)
        {
            return (ruleSet & expected) > 0;
        }
    }
}
