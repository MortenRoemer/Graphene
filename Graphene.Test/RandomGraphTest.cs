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
            var graph = RandomGraphs.GetRandomGraph(1, 0.5d, () => new MemoryGraph(), RandomGraphs.EdgeType.Hybrid,false);
            Assert.Equal(1uL, graph.Vertices.Count());
        }

        [Fact]
        public void NegativeGraphSizesShouldNotWork()
        {
            Assert.ThrowsAny<Exception>(() => RandomGraphs.GetRandomGraph(-42, 0.5d, () => new MemoryGraph(), RandomGraphs.EdgeType.Hybrid,false));
        }

        [Fact]
        public void EmptyGraphShouldWorkAndHaveNoVerticies()
        {
            var graph = RandomGraphs.GetRandomGraph(0, 0.5d, () => new MemoryGraph(), RandomGraphs.EdgeType.Hybrid,false);
            Assert.Equal(0uL, graph.Vertices.Count());
        }

        [Theory]
        [InlineData(10,false,45uL,9uL)]
        [InlineData(10,true,55uL,10uL)]
        public void CompleteBidirectionalGraphsShouldHaveAllEdges(int edgeCount, bool selfdirected, ulong expectedTotalEdgeCount, ulong expectedVertexEdgeCount)
        {
            var graph = RandomGraphs.GetRandomGraph(edgeCount, 1d, () => new MemoryGraph(), RandomGraphs.EdgeType.BidirectionalOnly,selfdirected);
            Assert.Equal(expectedTotalEdgeCount, graph.Edges.Count());
            foreach(var vertex in graph.Vertices)
            {
                Assert.Equal(expectedVertexEdgeCount, vertex.BidirectionalEdges.Count());
            }
        }

        [Theory]
        [InlineData(10, false, 90uL, 18uL)]
        [InlineData(10, true, 100uL, 20uL)]
        public void CompleteDirectionalGraphsShouldHaveAllEdges(int edgeCount, bool selfdirected, ulong expectedTotalEdgeCount, ulong expectedVertexEdgeCount)
        {
            var graph = RandomGraphs.GetRandomGraph(edgeCount, 1d, () => new MemoryGraph(), RandomGraphs.EdgeType.DirectedOnly,selfdirected);
            Assert.Equal(expectedTotalEdgeCount, graph.Edges.Count());
            foreach (var vertex in graph.Vertices)
            {
                Assert.Equal(expectedVertexEdgeCount, vertex.IngoingEdges.Count()+vertex.OutgoingEdges.Count());
            }
        }

        [Theory]
        [InlineData(10, false, 135uL, 27uL)]
        [InlineData(10, true, 155uL, 30uL)]
        public void CompleteHybridGraphsShouldHaveAllEdges(int edgeCount, bool selfdirected, ulong expectedTotalEdgeCount, ulong expectedVertexEdgeCount)
        {
            var graph = RandomGraphs.GetRandomGraph(edgeCount, 1d, () => new MemoryGraph(), RandomGraphs.EdgeType.Hybrid, selfdirected);
            Assert.Equal(expectedTotalEdgeCount, graph.Edges.Count());
            foreach (var vertex in graph.Vertices)
            {
                Assert.Equal(expectedVertexEdgeCount, vertex.IngoingEdges.Count() + vertex.OutgoingEdges.Count() + vertex.BidirectionalEdges.Count());
            }
        }
    }
}
