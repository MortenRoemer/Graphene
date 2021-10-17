using System;
using Graphene.InMemory;
using Xunit;

namespace Graphene.Test
{
    public class MemoryGraphTest
    {
        [Fact]
        public void NewGraphShouldBeEmpty()
        {
            IReadOnlyGraph graph = new MemoryGraph();
            var anyVertices = graph.Select().Vertices().Resolve(out _);
            Assert.False(anyVertices);
            var anyEdges = graph.Select().Edges().Resolve(out _);
            Assert.False(anyEdges);
        }

        [Fact]
        public void AttributeClearShouldWork()
        {
            var graph = new MemoryGraph();
            var entity = graph.Vertices.Create();
            var previousDataVersion = graph.DataVersion;
            entity.Attributes.Set("value", 1);
            Assert.NotEqual(previousDataVersion, graph.DataVersion);
            previousDataVersion = graph.DataVersion;
            entity.Attributes.Clear();
            Assert.NotEqual(previousDataVersion, graph.DataVersion);
            Assert.Empty(entity.Attributes);
        }

        [Fact]
        public void GenericEdgeEnumerationShouldGiveAllEdgesForAVertex()
        {
            IGraph graph = new MemoryGraph();
            IVertex va = graph.Vertices.Create();
            IVertex vb = graph.Vertices.Create();
            _ = va.BidirectionalEdges.Add(vb, null);
            _ = va.IngoingEdges.Add(vb, null);
            _ = va.OutgoingEdges.Add(vb, null);
            Assert.Equal(3, va.Edges.Count());
        }

        [Fact]
        public void GenericVertexEnumerationShouldGiveAllVerticesForAnEdge()
        {
            IGraph graph = new MemoryGraph();
            IVertex va = graph.Vertices.Create();
            IVertex vb = graph.Vertices.Create();
            IEdge ea = va.BidirectionalEdges.Add(vb, null);
            Assert.Equal(2, ea.Vertices.Count());
            IEdge eb = va.BidirectionalEdges.Add(va, null);
            Assert.Equal(1, eb.Vertices.Count());
        }
    }
}
