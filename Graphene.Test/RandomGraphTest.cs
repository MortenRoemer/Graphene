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
            var graph = RandomGraphs.GetRandomGraph(1, 0.5d, () => new MemoryGraph(), RandomGraphs.edgeType.hybrid,false);
            Assert.Equal(1, graph.Vertices.Count());
        }

        [Fact]
        public void NegativeGraphSizesShouldNotWork()
        {
            Assert.ThrowsAny<Exception>(() => RandomGraphs.GetRandomGraph(-42, 0.5d, () => new MemoryGraph(), RandomGraphs.edgeType.hybrid,false));
        }

        [Fact]
        public void EmptyGraphShouldWorkAndHaveNoVerticies()
        {
            var graph = RandomGraphs.GetRandomGraph(0, 0.5d, () => new MemoryGraph(), RandomGraphs.edgeType.hybrid,false);
            Assert.Equal(0, graph.Vertices.Count());
        }

        [Theory]
        [InlineData(10,false,45,9)]
        [InlineData(10,true,55,10)]
        public void CompleteBidirectionalGraphsShouldHaveAllEdges(int edgeCount, bool selfdirected, int expectedTotalEdgeCount, int expectedVertexEdgeCount)
        {
            var graph = RandomGraphs.GetRandomGraph(edgeCount, 1d, () => new MemoryGraph(), RandomGraphs.edgeType.bidirectedOnly,selfdirected);
            Assert.Equal(expectedTotalEdgeCount, graph.Edges.Count());
            foreach(var vertex in graph.Vertices)
            {
                Assert.Equal(expectedVertexEdgeCount, vertex.BidirectionalEdges.Count());
            }
        }

        [Theory]
        [InlineData(10, false, 90, 18)]
        [InlineData(10, true, 100, 20)]
        public void CompleteDirectionalGraphsShouldHaveAllEdges(int edgeCount, bool selfdirected, int expectedTotalEdgeCount, int expectedVertexEdgeCount)
        {
            var graph = RandomGraphs.GetRandomGraph(edgeCount, 1d, () => new MemoryGraph(), RandomGraphs.edgeType.directedOnly,selfdirected);
            Assert.Equal(expectedTotalEdgeCount, graph.Edges.Count());
            foreach (var vertex in graph.Vertices)
            {
                Assert.Equal(expectedVertexEdgeCount, vertex.IngoingEdges.Count()+vertex.OutgoingEdges.Count());
            }
        }

        [Theory]
        [InlineData(10, false, 135, 27)]
        [InlineData(10, true, 155, 30)]
        public void CompleteHybridGraphsShouldHaveAllEdges(int edgeCount, bool selfdirected, int expectedTotalEdgeCount, int expectedVertexEdgeCount)
        {
            var graph = RandomGraphs.GetRandomGraph(edgeCount, 1d, () => new MemoryGraph(), RandomGraphs.edgeType.hybrid, selfdirected);
            Assert.Equal(expectedTotalEdgeCount, graph.Edges.Count());
            foreach (var vertex in graph.Vertices)
            {
                Assert.Equal(expectedVertexEdgeCount, vertex.IngoingEdges.Count() + vertex.OutgoingEdges.Count() + vertex.BidirectionalEdges.Count());
            }
        }
    }
}
