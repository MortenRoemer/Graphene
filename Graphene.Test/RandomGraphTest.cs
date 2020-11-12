using Graphene.InMemory;
using Graphene.Random;
using System;
using Xunit;

namespace Graphene.Test
{
    public class RandomGraphTest
    {
        [Fact]
        public void MinimalGraphShouldHaveOneVertex()
        {
            var graph = new MemoryGraph();
            RandomGraphs.RandomizeGraph(1, 0.5d, graph, EdgeGenerationRule.NoEdges);
            Assert.Equal(1uL, graph.Vertices.Count());
        }

        [Fact]
        public void NegativeGraphSizesShouldNotWork()
        {
            var graph = new MemoryGraph();
            Assert.ThrowsAny<Exception>(() => RandomGraphs.RandomizeGraph(-42, 0.5d, graph, EdgeGenerationRule.NoEdges));
        }

        [Fact]
        public void EmptyGraphShouldWorkAndHaveNoVertices()
        {
            var graph = new MemoryGraph();
            RandomGraphs.RandomizeGraph(0, 0.5d, graph, EdgeGenerationRule.NoEdges);
            Assert.Equal(0uL, graph.Vertices.Count());
        }

        [Theory]
        [InlineData(10, EdgeGenerationRule.AllowUndirected, 45uL, 9uL)]
        [InlineData(10, EdgeGenerationRule.AllowUndirected | EdgeGenerationRule.AllowEdgesToItself, 55uL, 10uL)]
        [InlineData(10, EdgeGenerationRule.AllowDirected, 90uL, 18uL)]
        [InlineData(10, EdgeGenerationRule.AllowDirected | EdgeGenerationRule.AllowEdgesToItself, 100uL, 19uL)]
        [InlineData(10, EdgeGenerationRule.AllowDirected | EdgeGenerationRule.AllowUndirected, 135uL, 27uL)]
        [InlineData(10, EdgeGenerationRule.AllowDirected | EdgeGenerationRule.AllowUndirected | EdgeGenerationRule.AllowEdgesToItself, 155uL, 29uL)]
        public void CompleteGraphsShouldHaveAllEdges(int edgeCount, EdgeGenerationRule edgeGenerationRule, ulong expectedTotalEdgeCount, ulong expectedVertexEdgeCount)
        {
            var graph = new MemoryGraph();
            RandomGraphs.RandomizeGraph(edgeCount, 1d, graph, edgeGenerationRule);
            Assert.Equal(expectedTotalEdgeCount, graph.Edges.Count());
            
            foreach(var vertex in graph.Vertices)
            {
                Assert.Equal(expectedVertexEdgeCount, vertex.Edges.Count());
            }
        }
    }
}
