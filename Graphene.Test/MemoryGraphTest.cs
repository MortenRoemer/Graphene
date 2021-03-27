using System;
using System.Diagnostics;
using System.Linq;
using Graphene.InMemory;
using Xunit;

namespace Graphene.Test
{
    public class MemoryGraphTest
    {
        [Fact]
        public void NewGraphShouldBeEmpty()
        {
            var graph = new MemoryGraph();
            Assert.NotNull(graph.Vertices);
            Assert.Empty(graph.Vertices);
            Assert.NotNull(graph.Edges);
            Assert.Empty(graph.Edges);
            Assert.Equal(0, graph.Size);
        }

        [Fact]
        public void NewVertexShouldBeEmpty()
        {
            var graph = new MemoryGraph();
            var initialDataVersion = graph.DataVersion;
            var vertex = graph.Vertices.Create();
            Assert.NotNull(vertex.Graph);
            Assert.Null(vertex.Label);
            Assert.NotNull(vertex.Attributes);
            Assert.Empty(vertex.Attributes);
            Assert.NotNull(vertex.BidirectionalEdges);
            Assert.Empty(vertex.BidirectionalEdges);
            Assert.NotNull(vertex.IngoingEdges);
            Assert.Empty(vertex.IngoingEdges);
            Assert.NotNull(vertex.OutgoingEdges);
            Assert.Empty(vertex.OutgoingEdges);
            Assert.NotEqual(initialDataVersion,  graph.DataVersion);
        }

        [Fact]
        public void NewEdgeShouldBeEmpty()
        {
            var graph = new MemoryGraph();
            var fromVertex = graph.Vertices.Create();
            var toVertex = graph.Vertices.Create();
            var previousDataVersion = graph.DataVersion;
            
            var edge = fromVertex.OutgoingEdges.Add(toVertex);
            Assert.Equal(fromVertex.Id, edge.FromVertex.Id);
            Assert.Equal(toVertex.Id, edge.ToVertex.Id);
            Assert.NotNull(edge.Graph);
            Assert.True(edge.Directed);
            Assert.Null(edge.Label);
            Assert.NotNull(edge.Attributes);
            Assert.Empty(edge.Attributes);
            Assert.NotEqual(previousDataVersion, graph.DataVersion);
            previousDataVersion = graph.DataVersion;

            edge = fromVertex.IngoingEdges.Add(toVertex, null);
            Assert.Equal(fromVertex.Id, edge.ToVertex.Id);
            Assert.Equal(toVertex.Id, edge.FromVertex.Id);
            Assert.NotNull(edge.Graph);
            Assert.True(edge.Directed);
            Assert.Null(edge.Label);
            Assert.NotNull(edge.Attributes);
            Assert.Empty(edge.Attributes);
            Assert.NotEqual(previousDataVersion, graph.DataVersion);
            previousDataVersion = graph.DataVersion;

            edge = fromVertex.BidirectionalEdges.Add(toVertex, null);
            Assert.Equal(fromVertex.Id, edge.FromVertex.Id);
            Assert.Equal(toVertex.Id, edge.ToVertex.Id);
            Assert.NotNull(edge.Graph);
            Assert.False(edge.Directed);
            Assert.Null(edge.Label);
            Assert.NotNull(edge.Attributes);
            Assert.Empty(edge.Attributes);
            Assert.NotEqual(previousDataVersion, graph.DataVersion);
        }

        [Theory]
        [InlineData("lmao")]
        [InlineData(null)]
        public void LabelShouldStickToEntity(string label)
        {
            IGraph graph = new MemoryGraph();
            IEntity entity = graph.Vertices.Create(label);
            Assert.Equal(label, entity.Label);
        }

        [Fact]
        public void AttributesShouldStickToEntity()
        {
            var graph = new MemoryGraph();
            var entity = graph.Vertices.Create();
            var previousDataVersion = graph.DataVersion;
            entity.Attributes.Set("value", 5L);
            Assert.Equal(1L, entity.Attributes.Count);
            Assert.True(entity.Attributes.TryGet("value", out long value));
            Assert.Equal(5L, value);
            Assert.NotEqual(previousDataVersion, graph.DataVersion);
        }

        [Fact]
        public void MergeGraphShouldWork()
        {
            IGraph sourceGraph = new MemoryGraph();
            IVertex va = sourceGraph.Vertices.Create("va");
            va.Attributes.Set("value", 1);
            IVertex vb = sourceGraph.Vertices.Create("vb");
            vb.Attributes.Set("value", 2);
            IEdge ea = va.BidirectionalEdges.Add(vb, "ea");
            ea.Attributes.Set("value", 10);

            IGraph targetGraph = new MemoryGraph();
            IVertex vc = targetGraph.Vertices.Create("vc");
            vc.Attributes.Set("value", 3);
            IVertex vd = targetGraph.Vertices.Create("vd");
            vd.Attributes.Set("value", 4);
            IEdge eb = vc.BidirectionalEdges.Add(vd, "eb");
            eb.Attributes.Set("value", 20);

            targetGraph.Merge(sourceGraph);

            using var vertices = targetGraph.Vertices
                .OrderBy(vertex => vertex.Label)
                .GetEnumerator();

            Assert.True(vertices.MoveNext());
            Assert.Equal("va", vertices.Current.Label);
            Assert.Equal(1, vertices.Current.Attributes.TryGet("value", out int valueA) ? valueA : throw new Exception());
            Assert.True(vertices.MoveNext());
            Assert.Equal("vb", vertices.Current.Label);
            Assert.Equal(2, vertices.Current.Attributes.TryGet("value", out int valueB) ? valueB : throw new Exception());
            Assert.True(vertices.MoveNext());
            Assert.Equal("vc", vertices.Current.Label);
            Assert.Equal(3, vertices.Current.Attributes.TryGet("value", out int valueC) ? valueC : throw new Exception());
            Assert.True(vertices.MoveNext());
            Assert.Equal("vd", vertices.Current.Label);
            Assert.Equal(4, vertices.Current.Attributes.TryGet("value", out int valueD) ? valueD : throw new Exception());
            Assert.False(vertices.MoveNext());

            using var edges = targetGraph.Edges
                .OrderBy(edge => edge.Label)
                .GetEnumerator();

            Assert.True(edges.MoveNext());
            Assert.Equal("ea", edges.Current.Label);
            Assert.Equal(10, edges.Current.Attributes.TryGet("value", out int valueE) ? valueE : throw new Exception());
            Assert.True(edges.MoveNext());
            Assert.Equal("eb", edges.Current.Label);
            Assert.Equal(20, edges.Current.Attributes.TryGet("value", out int valueF) ? valueF : throw new Exception());
            Assert.False(edges.MoveNext());
        }

        [Fact]
        public void CloneGraphShouldWork()
        {
            IGraph sourceGraph = new MemoryGraph();
            IVertex va = sourceGraph.Vertices.Create("va");
            va.Attributes.Set("value", 1);
            IVertex vb = sourceGraph.Vertices.Create("vb");
            vb.Attributes.Set("value", 2);
            IEdge ea = va.BidirectionalEdges.Add(vb, "ea");
            ea.Attributes.Set("value", 10);

            IGraph targetGraph = sourceGraph.Clone();

            using var vertices = targetGraph.Vertices
                .OrderBy(vertex => vertex.Label)
                .GetEnumerator();

            Assert.True(vertices.MoveNext());
            Assert.Equal("va", vertices.Current.Label);
            Assert.Equal(1, vertices.Current.Attributes.TryGet("value", out int valueA) ? valueA : throw new Exception());
            Assert.True(vertices.MoveNext());
            Assert.Equal("vb", vertices.Current.Label);
            Assert.Equal(2, vertices.Current.Attributes.TryGet("value", out int valueB) ? valueB : throw new Exception());
            Assert.False(vertices.MoveNext());

            using var edges = targetGraph.Edges.GetEnumerator();

            Assert.True(edges.MoveNext());
            Assert.Equal("ea", edges.Current.Label);
            Assert.Equal(10, edges.Current.Attributes.TryGet("value", out int valueC) ? valueC : throw new Exception());
            Assert.False(edges.MoveNext());
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
