using System;
using System.Linq;
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
        public void AttributeHasNoValueFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyVertex()
                .Where()
                    .Attribute("hanseatic").HasNoValue()
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.DoesNotContain("hanseatic", entity.Attributes.Select(attribute => attribute.Key));

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.DoesNotContain("hanseatic", entity.Attributes.Select(attribute => attribute.Key));
            }
        }

        [Fact]
        public void AttributeHasValueFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyVertex()
                .Where()
                    .Attribute("hanseatic").HasValue()
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.Contains("hanseatic", entity.Attributes.Select(attribute => attribute.Key));

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.Contains("hanseatic", entity.Attributes.Select(attribute => attribute.Key));
            }
        }

        [Fact]
        public void AttributeIsBetweenFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyEdges()
                .Where()
                    .Attribute("distance").IsBetween(100, 200)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.True(entity.Attributes.TryGet("distance", out int distance));
            Assert.InRange(distance, 100, 200);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.True(entity.Attributes.TryGet("distance", out distance));
                Assert.InRange(distance, 100, 200);
            }
        }

        [Fact]
        public void AttributeIsEqualToFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyEdges()
                .Where()
                    .Attribute("distance").IsEqualTo(232)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.True(entity.Attributes.TryGet("distance", out int distance));
            Assert.Equal(232, distance);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.True(entity.Attributes.TryGet("distance", out distance));
                Assert.Equal(232, distance);
            }
        }

        [Fact]
        public void AttributeIsGreaterOrEqualToFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyEdges()
                .Where()
                    .Attribute("distance").IsGreaterOrEqualTo(232)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.True(entity.Attributes.TryGet("distance", out int distance));
            Assert.True(distance >= 232);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.True(entity.Attributes.TryGet("distance", out distance));
                Assert.True(distance >= 232);
            }
        }

        [Fact]
        public void AttributeIsGreaterThanFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyEdges()
                .Where()
                    .Attribute("distance").IsGreaterThan(232)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.True(entity.Attributes.TryGet("distance", out int distance));
            Assert.True(distance > 232);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.True(entity.Attributes.TryGet("distance", out distance));
                Assert.True(distance > 232);
            }
        }

        [Fact]
        public void AttributeIsInFilterShouldGiveCorrectEntities()
        {
            var distances = new[] { 232, 367 };
            var query = ExampleGraph.Value
                .Select()
                .AnyEdges()
                .Where()
                    .Attribute("distance").IsIn(distances)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.True(entity.Attributes.TryGet("distance", out int distance));
            Assert.Contains(distance, distances);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.True(entity.Attributes.TryGet("distance", out distance));
                Assert.Contains(distance, distances);
            }
        }

        [Fact]
        public void AttributeIsLessOrEqualToFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyEdges()
                .Where()
                    .Attribute("distance").IsLessOrEqualTo(232)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.True(entity.Attributes.TryGet("distance", out int distance));
            Assert.True(distance <= 232);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.True(entity.Attributes.TryGet("distance", out distance));
                Assert.True(distance <= 232);
            }
        }

        [Fact]
        public void AttributeIsLessThanFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyEdges()
                .Where()
                    .Attribute("distance").IsLessThan(232)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.True(entity.Attributes.TryGet("distance", out int distance));
            Assert.True(distance < 232);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.True(entity.Attributes.TryGet("distance", out distance));
                Assert.True(distance < 232);
            }
        }

        [Fact]
        public void AttributeIsNotBetweenFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyEdges()
                .Where()
                    .Attribute("distance").IsNotBetween(100, 200)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.True(entity.Attributes.TryGet("distance", out int distance));
            Assert.NotInRange(distance, 100, 200);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.True(entity.Attributes.TryGet("distance", out distance));
                Assert.NotInRange(distance, 100, 200);
            }
        }

        [Fact]
        public void AttributeIsNotEqualToFilterShouldGiveCorrectEntities()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyEdges()
                .Where()
                    .Attribute("distance").IsNotEqualTo(232)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.True(entity.Attributes.TryGet("distance", out int distance));
            Assert.NotEqual(232, distance);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.True(entity.Attributes.TryGet("distance", out distance));
                Assert.NotEqual(232, distance);
            }
        }

        [Fact]
        public void AttributeIsNotInFilterShouldGiveCorrectEntities()
        {
            var distances = new[] { 232, 367 };
            var query = ExampleGraph.Value
                .Select()
                .AnyEdges()
                .Where()
                    .Attribute("distance").IsNotIn(distances)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Single(result);
            var entity = result.First();
            Assert.True(entity.Attributes.TryGet("distance", out int distance));
            Assert.DoesNotContain(distance, distances);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.True(entity.Attributes.TryGet("distance", out distance));
                Assert.DoesNotContain(distance, distances);
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
            Assert.DoesNotMatch("ha.+", entity.Label);

            while (result.FindNextResult(out result))
            {
                Assert.Single(result);
                entity = result.First();
                Assert.DoesNotMatch("ha.+", entity.Label);
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

        [Fact]
        public void RelativeQueryShouldGiveCorrectSolutions()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyEdges()
                .TargetVertex()
                .Where()
                    .Attribute("hanseatic").IsEqualTo(true)
                .EndWhere();

            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Equal(2, result.Count());

            using (var enumerator = result.GetEnumerator())
            {
                IVertex vertex = null;
                IEdge edge = null;

                Assert.True(enumerator.MoveNext());
                Assert.True(enumerator.Current is IEdge);
                edge = enumerator.Current as IEdge;
                Assert.True(enumerator.MoveNext());
                Assert.True(enumerator.Current is IVertex);
                vertex = enumerator.Current as IVertex;

                if (edge.Directed)
                    Assert.Equal(vertex.Id, edge.ToVertex.Id);
                else
                    Assert.Contains(vertex.Id, new[] { edge.FromVertex.Id, edge.ToVertex.Id });
                
                Assert.True(vertex.Attributes.TryGet("hanseatic", out bool isHanseatic));
                Assert.True(isHanseatic);
                Assert.False(enumerator.MoveNext());
            }

            while (result.FindNextResult(out result))
            {
                using (var enumerator = result.GetEnumerator())
                {
                    IVertex vertex = null;
                    IEdge edge = null;

                    Assert.True(enumerator.MoveNext());
                    Assert.True(enumerator.Current is IEdge);
                    edge = enumerator.Current as IEdge;
                    Assert.True(enumerator.MoveNext());
                    Assert.True(enumerator.Current is IVertex);
                    vertex = enumerator.Current as IVertex;

                    if (edge.Directed)
                        Assert.Equal(vertex.Id, edge.ToVertex.Id);
                    else
                        Assert.Contains(vertex.Id, new[] { edge.FromVertex.Id, edge.ToVertex.Id });
                    
                    Assert.True(vertex.Attributes.TryGet("hanseatic", out bool isHanseatic));
                    Assert.True(isHanseatic);
                    Assert.False(enumerator.MoveNext());
                }
            }
        }

        [Fact]
        public void RouteQueryShouldGiveCorrectResults()
        {
            var query = ExampleGraph.Value
                .Select()
                .AnyVertex()
                .Where()
                    .Label().IsEqualTo("hamburg")
                .EndWhere()
                .RouteToAnyVertex()
                .Where()
                    .Label().IsEqualTo("cologne")
                .EndWhere()
                .WhereAnyHopEdge()
                    .Attribute("distance").IsGreaterThan(0)
                .EndWhere()
                .OptimizeSoThat().TheSumOf().Attribute("distance").IsMinimal();
            
            Assert.True(query.Resolve(out IQueryResult result));
            Assert.Equal(5, result.Count());

            using(var enumerator = result.GetEnumerator())
            {
                Assert.True(enumerator.MoveNext());
                Assert.True(enumerator.Current is IVertex);
                Assert.Equal("hamburg", enumerator.Current.Label);
                Assert.True(enumerator.MoveNext());
                Assert.True(enumerator.Current is IEdge);
                Assert.Equal("a7", enumerator.Current.Label);
                Assert.True(enumerator.MoveNext());
                Assert.True(enumerator.Current is IVertex);
                Assert.Equal("hannover", enumerator.Current.Label);
                Assert.True(enumerator.MoveNext());
                Assert.True(enumerator.Current is IEdge);
                Assert.Equal("a2", enumerator.Current.Label);
                Assert.True(enumerator.MoveNext());
                Assert.True(enumerator.Current is IVertex);
                Assert.Equal("cologne", enumerator.Current.Label);
                Assert.False(enumerator.MoveNext());
            }

            Assert.False(result.FindNextResult(out _));
        }

        private static IGraph PrepareGraph()
        {
            IGraph graph = new MemoryGraph();

            IVertex hamburg = graph.Vertices.Create(nameof(hamburg));
            hamburg.Attributes.Set("hanseatic", true);

            IVertex hannover = graph.Vertices.Create(nameof(hannover));

            IVertex berlin = graph.Vertices.Create(nameof(berlin));

            IVertex dresden = graph.Vertices.Create(nameof(dresden));

            IVertex cologne = graph.Vertices.Create(nameof(cologne));
            cologne.Attributes.Set("hanseatic", true);

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