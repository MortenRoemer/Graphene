using System;
using System.Linq;
using System.Threading;
using Graphene.InMemory;
using Graphene.Test.Utility;
using Xunit;

namespace Graphene.Test
{
    public class QueryTest
    {
        private static readonly Lazy<IGraph> ExampleGraph = new Lazy<IGraph>(CreateExampleGraph, LazyThreadSafetyMode.ExecutionAndPublication);

        [Fact]
        public void VerticesTest()
        {
            var graph = ExampleGraph.Value;

            Assert.True(graph.Select()
                .Vertices()
                .Where(vertex => vertex.Label == "city")
                .Resolve(out var result));
            
            Assert.True(result.Entities.All(vertex => vertex.Label == "city"));
            Assert.False(result.FindNextPage(out result));
        }
        
        [Fact]
        public void EdgesTest()
        {
            var graph = ExampleGraph.Value;

            Assert.True(graph.Select()
                .Edges()
                .Where(edge => edge.Label == "autobahn")
                .Resolve(out var result));
            
            Assert.True(result.Entities.All(edge => edge.Label == "autobahn"));
            Assert.False(result.FindNextPage(out result));
        }
        
        [Fact]
        public void RouteVertexWithMinimalEdgesToVertexTest()
        {
            var graph = ExampleGraph.Value;
            var hamburgId = GetVertexWithName(graph, "hamburg");
            var berlinId = GetVertexWithName(graph, "berlin");

            var route = graph.Select()
                .Route()
                .FromVertex(hamburgId)
                .WithMinimalEdges()
                .ToVertex(berlinId)
                .Resolve();
            
            Assert.True(route.Found);
            Assert.Equal(1, route.Metric);
            Assert.Equal("hamburg", route.Origin.Attributes.GetOrDefault("name", string.Empty));
            Assert.Equal("a24", route.Steps[0].Edge.Attributes.GetOrDefault("name", string.Empty));
            Assert.Equal("berlin", route.Steps[0].Vertex.Attributes.GetOrDefault("name", string.Empty));
        }
        
        [Fact]
        public void RouteVertexWithMinimalMetricToVertexTest()
        {
            var graph = ExampleGraph.Value;
            var hamburgId = GetVertexWithName(graph, "hamburg");
            var berlinId = GetVertexWithName(graph, "berlin");

            var route = graph.Select()
                .Route()
                .FromVertex(hamburgId)
                .WithMinimalMetric(
                    edge => edge.Attributes.GetOrDefault("distance", float.PositiveInfinity),
                    (first, second) => first + second)
                .ToVertex(berlinId)
                .Resolve();
            
            Assert.True(route.Found);
            Assert.Equal(237.46f, route.Metric);
            Assert.Equal("hamburg", route.Origin.Attributes.GetOrDefault("name", string.Empty));
            Assert.Equal("a24", route.Steps[0].Edge.Attributes.GetOrDefault("name", string.Empty));
            Assert.Equal("berlin", route.Steps[0].Vertex.Attributes.GetOrDefault("name", string.Empty));
        }
        
        [Fact]
        public void RouteVertexWithMinimalMetricAndHeuristicToVertexTest()
        {
            var graph = ExampleGraph.Value;
            var hamburgId = GetVertexWithName(graph, "hamburg");
            var berlinId = GetVertexWithName(graph, "berlin");

            var route = graph.Select()
                .Route()
                .FromVertex(hamburgId)
                .WithMinimalMetric(
                    edge => edge.Attributes.GetOrDefault("distance", float.PositiveInfinity),
                    (first, second) => first + second)
                .WithHeuristic((fromVertex, toVertex) =>
                    (float) fromVertex.Attributes.GetOrDefault("position", new Coordinate())
                        .CalcDistanceTo(toVertex.Attributes.GetOrDefault("position", new Coordinate())) / 1000)
                .ToVertex(berlinId)
                .Resolve();
            
            Assert.True(route.Found);
            Assert.Equal(237.46f, route.Metric);
            Assert.Equal("hamburg", route.Origin.Attributes.GetOrDefault("name", string.Empty));
            Assert.Equal("a24", route.Steps[0].Edge.Attributes.GetOrDefault("name", string.Empty));
            Assert.Equal("berlin", route.Steps[0].Vertex.Attributes.GetOrDefault("name", string.Empty));
        }
        
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
                edge.Attributes.TryGet("distance", out float distance) && distance < 300.00f;

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
                edge.Attributes.TryGet("distance", out float distance) && distance < 300.00f;

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
            berlin.Attributes["position"] = new Coordinate(52.518611, 13.408333);

            var hamburg = graph.Vertices.Create("city");
            hamburg.Attributes["name"] = nameof(hamburg);
            hamburg.Attributes["population"] = 1_899_160;
            hamburg.Attributes["position"] = new Coordinate(53.550556, 9.993333);

            var munich = graph.Vertices.Create("city");
            munich.Attributes["name"] = nameof(munich);
            munich.Attributes["population"] = 1_484_226;
            munich.Attributes["position"] = new Coordinate(48.139609,11.565949);
            
            var a24 = hamburg.BidirectionalEdges.Add(berlin, "autobahn");
            a24.Attributes["name"] = nameof(a24);
            a24.Attributes["distance"] = 237.46f;

            var a7 = hamburg.BidirectionalEdges.Add(munich, "autobahn");
            a7.Attributes["name"] = nameof(a7);
            a7.Attributes["distance"] = 777.52f;

            var a9 = berlin.BidirectionalEdges.Add(munich, "autobahn");
            a9.Attributes["name"] = nameof(a9);
            a9.Attributes["distance"] = 584.81f;

            return graph;
        }

        private static int GetVertexWithName(IReadOnlyGraph graph, string expectedName)
        {
            return graph.Vertices
                .Where(vertex => vertex.Attributes.TryGet("name", out string name) && name.Equals(expectedName))
                .Select(vertex => vertex.Id)
                .First();
        }
    }
}