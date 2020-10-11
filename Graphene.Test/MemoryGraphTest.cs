using Graphene;
using Graphene.InMemory;
using Xunit;

namespace Graphene.Test
{
    public class MemoryGraphTest
    {
        [Fact]
        public void NewGraphShouldBeEmpty()
        {
            IGraph graph = new MemoryGraph();
            Assert.NotNull(graph.Vertices);
            Assert.Empty(graph.Vertices);
            Assert.NotNull(graph.Edges);
            Assert.Empty(graph.Edges);
            Assert.Equal(0, graph.Size);
        }

        [Fact]
        public void NewVertexShouldBeEmpty()
        {
            IGraph graph = new MemoryGraph();
            IVertex vertex = graph.Vertices.Create();
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
        }

        [Fact]
        public void NewEdgeShouldBeEmpty()
        {
            IGraph graph = new MemoryGraph();
            IVertex fromVertex = graph.Vertices.Create();
            IVertex toVertex = graph.Vertices.Create();
            IEdge edge = fromVertex.OutgoingEdges.Add(toVertex);
            Assert.Equal(fromVertex.Id, edge.FromVertex.Id);
            Assert.Equal(toVertex.Id, edge.ToVertex.Id);
            Assert.NotNull(edge.Graph);
            Assert.True(edge.Directed);
            Assert.Null(edge.Label);
            Assert.NotNull(edge.Attributes);
            Assert.Empty(edge.Attributes);
        }

        [Theory]
        [InlineData("lol")]
        [InlineData(null)]
        public void LabelShouldStickToEntity(string label)
        {
            IGraph graph = new MemoryGraph();
            IEntity fromVertex = graph.Vertices.Create();
        }
    }
}
