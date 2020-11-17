using System;
using System.Linq;
using System.Threading;
using Graphene.InMemory;
using Xunit;

namespace Graphene.Test
{
    public class QueryTest
    {
        private static readonly Lazy<IGraph> ExampleGraph = new Lazy<IGraph>(CreateExampleGraph, LazyThreadSafetyMode.ExecutionAndPublication);

        [Fact]
        public void SubGraphSelectVerticesTest()
        {
            var graph = ExampleGraph.Value;

            var subGraph = graph.Select()
                .SubGraph()
                .Vertices()
                .Resolve();
            
            Assert.Equal(graph.Vertices.Count(), subGraph.Vertices.Count());
            Assert.False(subGraph.Edges.Any());
        }
        
        [Fact]
        public void SubGraphSelectEdgesTest()
        {
            var graph = ExampleGraph.Value;

            var subGraph = graph.Select()
                .SubGraph()
                .Edges()
                .Resolve();
            
            Assert.Equal(graph.Vertices.Count(), subGraph.Vertices.Count());
            Assert.Equal(graph.Edges.Count(), subGraph.Edges.Count());
        }
        
        [Fact]
        public void SubGraphSelectVerticesWithEdgesTest()
        {
            var graph = ExampleGraph.Value;

            var subGraph = graph.Select()
                .SubGraph()
                .Vertices()
                .WithEdges()
                .Resolve();
            
            Assert.Equal(graph.Vertices.Count(), subGraph.Vertices.Count());
            Assert.Equal(graph.Edges.Count(), subGraph.Edges.Count());
        }
        
        [Fact]
        public void SubGraphVertexFilterTest()
        {
            static bool Filter(IReadOnlyVertex vertex) =>
                vertex.Attributes.TryGet("population", out int population) && population >= 1_000_000;
            
            var graph = ExampleGraph.Value;

            var subGraph = graph.Select()
                .SubGraph()
                .Vertices()
                .Where(Filter)
                .Resolve();

            Assert.True(subGraph.Size > 0);
            Assert.True(subGraph.Vertices.Any());
            Assert.False(subGraph.Edges.Any());

            foreach (var vertex in subGraph.Vertices)
            {
                Assert.True(Filter(vertex));
            }
        }

        [Fact]
        public void SubGraphEdgesFilterTest()
        {
            static bool Filter(IReadOnlyEdge edge) =>
                edge.Attributes.TryGet("distance", out double distance) && distance < 300.00;

            var graph = ExampleGraph.Value;

            var subGraph = graph.Select()
                .SubGraph()
                .Edges()
                .Where(Filter)
                .Resolve();
            
            Assert.True(subGraph.Size > 0);
            Assert.True(subGraph.Vertices.Any());
            Assert.True(subGraph.Edges.Any());

            foreach (var edge in subGraph.Edges)
            {
                Assert.True(Filter(edge));
            }
        }
        
        [Fact]
        public void SubGraphVertexWithEdgesFilterTest()
        {
            static bool Filter(IReadOnlyEdge edge) =>
                edge.Attributes.TryGet("distance", out double distance) && distance < 300.00;

            var graph = ExampleGraph.Value;

            var subGraph = graph.Select()
                .SubGraph()
                .Vertices()
                .WithEdges()
                .Where(Filter)
                .Resolve();
            
            Assert.True(subGraph.Size > 0);
            Assert.True(subGraph.Vertices.Any());
            Assert.True(subGraph.Edges.Any());

            foreach (var edge in subGraph.Edges)
            {
                Assert.True(Filter(edge));
            }
        }

        private static IGraph CreateExampleGraph()
        {
            var graph = new MemoryGraph();

            var berlin = graph.Vertices.Create("city");
            berlin.Attributes["name"] = nameof(berlin);
            berlin.Attributes["population"] = 3_669_491;

            var hamburg = graph.Vertices.Create("city");
            hamburg.Attributes["name"] = nameof(hamburg);
            hamburg.Attributes["population"] = 1_899_160;

            var munich = graph.Vertices.Create("city");
            munich.Attributes["name"] = munich;
            munich.Attributes["population"] = 1_484_226;
            
            var a24 = hamburg.BidirectionalEdges.Add(berlin, "autobahn");
            a24.Attributes["name"] = nameof(a24);
            a24.Attributes["distance"] = 237.46;

            var a7 = hamburg.BidirectionalEdges.Add(munich, "autobahn");
            a7.Attributes["name"] = nameof(a7);
            a7.Attributes["distance"] = 777.52;

            var a9 = berlin.BidirectionalEdges.Add(munich, "autobahn");
            a9.Attributes["name"] = nameof(a9);
            a9.Attributes["distance"] = 584.81;

            return graph;
        }
    }
}