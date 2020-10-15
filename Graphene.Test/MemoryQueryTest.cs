using System;
using System.Linq;
using System.Text.RegularExpressions;
using Graphene.InMemory;
using Graphene.Query;
using Xunit;

namespace Graphene.Test
{
    public class MemoryQueryTest
    {
        private static readonly Lazy<IGraph> ExampleGraph = new Lazy<IGraph>(() => PrepareGraph(), isThreadSafe: true);

        [Fact]
        public void EmptyVertexQueryShouldGiveAnyVertexInOrder()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyVertex();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.True(entity is IVertex);
            var lastid = entity.Id;

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.True(entity is IVertex);
                Assert.True(entity.Id > lastid);
                lastid = entity.Id;
            }
        }

        [Fact]
        public void LabelIsEqualToFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyVertex()
                .Where()
                    .Label().IsEqualTo("berlin")
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.Equal("berlin", entity.Label);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.Equal("berlin", entity.Label);
            }
        }

        [Fact]
        public void LabelIsNotEqualToFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyVertex()
                .Where()
                    .Label().IsNotEqualTo("berlin")
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.NotEqual("berlin", entity.Label);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.NotEqual("berlin", entity.Label);
            }
        }

        [Fact]
        public void LabelDoesMatchFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyVertex()
                .Where()
                    .Label().DoesMatch("ha.+")
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.Matches("ha.+", entity.Label);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.Matches("ha.+", entity.Label);
            }
        }

        [Fact]
        public void LabelDoesNotMatchFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyVertex()
                .Where()
                    .Label().DoesNotMatch("ha.+")
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.False(Regex.IsMatch(entity.Label, "ha.+"));

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.False(Regex.IsMatch(entity.Label, "ha.+"));
            }
        }

        [Fact]
        public void LabelIsInFilterShouldGiveCorrectEntities()
        {
            var labels = new[] {"hamburg", "berlin"};
            var query = ExampleGraph.Value
                .Select()
                .AnyVertex()
                .Where()
                    .Label().IsIn(labels)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.Contains(entity.Label, labels);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.Contains(entity.Label, labels);
            }
        }

        [Fact]
        public void LabelIsNotInFilterShouldGiveCorrectEntities()
        {
            var labels = new[] {"hamburg", "berlin"};
            var query = ExampleGraph.Value
                .Select()
                .AnyVertex()
                .Where()
                    .Label().IsNotIn(labels)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.DoesNotContain(entity.Label, labels);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.DoesNotContain(entity.Label, labels);
            }
        }

        [Fact]
        public void EmptyEdgeQueryShouldGiveAnyEdgeInOrder()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyEdges();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.True(entity is IEdge);
            var lastid = entity.Id;

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.True(entity is IEdge);
                Assert.True(entity.Id > lastid);
                lastid = entity.Id;
            }
        }

        private static IGraph PrepareGraph()
        {
            IGraph graph = new MemoryGraph();
            IVertex hamburg = graph.Vertices.Create(nameof(hamburg));
            IVertex hannover = graph.Vertices.Create(nameof(hannover));
            IVertex berlin = graph.Vertices.Create(nameof(berlin));
            IVertex dresden = graph.Vertices.Create(nameof(dresden));
            IVertex cologne = graph.Vertices.Create(nameof(cologne));
            IVertex muenchen = graph.Vertices.Create(nameof(muenchen));
            IVertex stuttgart = graph.Vertices.Create(nameof(stuttgart));
            
            IEdge a7 = hamburg.BidirectionalEdges.Add(hannover, nameof(a7));
            a7.Attributes.Set("distance", 151);

            IEdge a24 = hamburg.BidirectionalEdges.Add(berlin, nameof(a24));
            a24.Attributes.Set("distance", 289);

            IEdge a2 = hannover.BidirectionalEdges.Add(cologne, nameof(a2));
            a2.Attributes.Set("distance", 293);

            IEdge a22 = hannover.BidirectionalEdges.Add(berlin, nameof(a22));
            a22.Attributes.Set("distance", 285);

            IEdge a13 = berlin.BidirectionalEdges.Add(dresden, nameof(a13));
            a13.Attributes.Set("distance", 224);

            IEdge a9 = dresden.BidirectionalEdges.Add(muenchen, nameof(a9));
            a9.Attributes.Set("distance", 460);

            IEdge a8 = muenchen.BidirectionalEdges.Add(stuttgart, nameof(a8));
            a8.Attributes.Set("distance", 232);

            IEdge a3 = stuttgart.BidirectionalEdges.Add(cologne, nameof(a3));
            a3.Attributes.Set("distance", 367);

            return graph;
        }
    }
}