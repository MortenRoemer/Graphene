using FluentAssertions;
using Graphene.InMemory;
using Graphene.Transactions;
using System;
using System.Threading.Tasks;
using Graphene.Test.Utility;
using Xunit;

namespace Graphene.Test
{
    public class MemoryGraphTest
    {
        [Fact]
        public async void CreatingVerticesAndEdgesShouldWork()
        {
            var graph = await PrepareExampleGraph(nameof(CreatingVerticesAndEdgesShouldWork));
            var bremen = new Vertex(CityLabel);
            var a1 = new Edge(HighwayLabel, bremen.Id, HamburgId, false);

            await graph.Execute(new Transaction
            {
                bremen.ToCreateVertexAction(),
                a1.ToCreateEdgeAction()
            });

            var cities = await graph.FindVertices(5, vertex => vertex.Label == CityLabel);
            cities.Results.Should().HaveCount(4);

            var highways = await graph.FindEdges(5, edge => edge.Label == HighwayLabel);
            highways.Results.Should().HaveCount(4);
        }

        [Fact]
        public async void EmptyGuidsShouldBeRejected()
        {
            var graph = await PrepareExampleGraph(nameof(EmptyGuidsShouldBeRejected));
            var bremen = new Vertex(CityLabel, Guid.Empty);
            var a1 = new Edge(HighwayLabel, bremen.Id, HamburgId, false, Guid.Empty);
            Exception? exception = null;
            
            try
            {
                await graph.Execute(new Transaction
                {
                    bremen.ToCreateVertexAction(),
                    a1.ToCreateEdgeAction()
                });
            }
            catch (Exception caughtException)
            {
                exception = caughtException;
            }
            
            exception.Should().NotBeNull().And.BeOfType<GraphActionException>();
            exception!.Message.Should().Contain("empty Guid is not valid");
        }

        [Fact]
        public async void CreatingDuplicateGuidsShouldBeRejected()
        {
            var graph = await PrepareExampleGraph(nameof(CreatingDuplicateGuidsShouldBeRejected));
            var anotherHamburg = new Vertex(CityLabel, HamburgId);
            Exception? exception = null;

            try
            {
                await graph.Execute(new Transaction
                {
                    anotherHamburg.ToCreateVertexAction()
                });
            }
            catch (Exception caughtException)
            {
                exception = caughtException;
            }
            
            exception.Should().NotBeNull().And.BeOfType<GraphActionException>();
            exception!.Message.Should().Contain("there is already an entity with id");
        }

        [Fact]
        public async void UpdatingEntitiesShouldWork()
        {
            var graph = await PrepareExampleGraph(nameof(UpdatingEntitiesShouldWork));
            var hamburgPatch = new Vertex(CityLabel, HamburgId)
                .WithAttribute(PopulationLabel, 2000000)
                .WithAttribute("Metropolis", true);
            var a24Patch = new Edge(HighwayLabel, HamburgId, BerlinId, false, A24Id)
                .WithAttribute(DistanceLabel, 300.0)
                .WithAttribute("UnderConstruction", true);

            await graph.Execute(new Transaction
            {
                hamburgPatch.ToUpdateEntityAction(),
                a24Patch.ToUpdateEntityAction()
            });

            var patchedHamburg = await graph.Get(HamburgId);
            patchedHamburg.Get<string>(NameLabel).Should().Be("Hamburg");
            patchedHamburg.Get<int>(PopulationLabel).Should().Be(2000000);
            patchedHamburg.Get<bool>("Metropolis").Should().Be(true);

            var patchedA24 = await graph.Get(A24Id);
            patchedA24.Get<string>(NameLabel).Should().Be("A24");
            patchedA24.Get<double>(DistanceLabel).Should().Be(300.0);
            patchedA24.Get<bool>("UnderConstruction").Should().Be(true);
        }

        [Fact]
        public async void UpdatesToMissingEntitiesShouldFail()
        {
            var graph = await PrepareExampleGraph(nameof(UpdatesToMissingEntitiesShouldFail));
            var misguidedPatch = new Vertex(CityLabel);
            Exception? exception = null;

            try
            {
                await graph.Execute(new Transaction
                {
                    misguidedPatch.ToUpdateEntityAction()
                });
            }
            catch (Exception caughtException)
            {
                exception = caughtException;
            }
            
            exception.Should().NotBeNull().And.BeOfType<GraphActionException>();
            exception!.Message.Should().Contain("there is no entity with id");
        }

        [Fact]
        public async void DeletingEntitiesShouldWork()
        {
            var graph = await PrepareExampleGraph(nameof(DeletingEntitiesShouldWork));
            var hamburg = new Vertex(CityLabel, HamburgId);

            await graph.Execute(new Transaction
            {
                hamburg.ToDeleteEntityAction()
            });
            
            var cities = await graph.FindVertices(3, vertex => vertex.Label == CityLabel);
            cities.Results.Should().HaveCount(2);

            var highways = await graph.FindEdges(2, edge => edge.Label == HighwayLabel);
            highways.Results.Should().HaveCount(1);
        }

        [Fact]
        public async void FindingShortestRouteByEdgeCountShouldWork()
        {
            var graph = await PrepareExampleGraph(nameof(FindingShortestRouteByEdgeCountShouldWork));

            var route = await graph.Select().Route()
                .FromVertex(HamburgId)
                .WithMinimalEdges()
                .Where(edge => edge.Label == HighwayLabel)
                .ToVertex(BerlinId)
                .Resolve();

            route.Cost.Should().Be(1);
            route.Steps[0].Edge.Id.Should().Be(A24Id);
        }
        
        [Fact]
        public async void FindingShortestRouteByDistanceCountShouldWork()
        {
            var graph = await PrepareExampleGraph(nameof(FindingShortestRouteByDistanceCountShouldWork));

            var route = await graph.Select().Route()
                .FromVertex(HamburgId)
                .WithMinimalMetric(edge => edge.Get<double>(DistanceLabel), 0.0, (left, right) => left + right)
                .Where(edge => edge.Label == HighwayLabel)
                .ToVertex(BerlinId)
                .Resolve();

            route.Cost.Should().Be(289.0);
            route.Steps[0].Edge.Id.Should().Be(A24Id);
        }
        
        [Fact]
        public async void FindingShortestRouteByHeuristicShouldWork()
        {
            var graph = await PrepareExampleGraph(nameof(FindingShortestRouteByHeuristicShouldWork));

            var route = await graph.Select().Route()
                .FromVertex(HamburgId)
                .WithMinimalMetric(edge => edge.Get<double>(DistanceLabel), 0.0, (left, right) => left + right)
                .Where(edge => edge.Label == HighwayLabel)
                .WithHeuristic((fromVertex, toVertex) => fromVertex.Get<Coordinate>(CoordinatesLabel).CalcDistanceTo(toVertex.Get<Coordinate>(CoordinatesLabel)))
                .ToVertex(BerlinId)
                .Resolve();

            route.Cost.Should().Be(289.0);
            route.Steps[0].Edge.Id.Should().Be(A24Id);
        }
        
        [Fact]
        public async void DeletionsOfMissingEntitiesShouldFail()
        {
            var graph = await PrepareExampleGraph(nameof(UpdatesToMissingEntitiesShouldFail));
            var misguidedDeletion = new Vertex(CityLabel);
            Exception? exception = null;

            try
            {
                await graph.Execute(new Transaction
                {
                    misguidedDeletion.ToDeleteEntityAction()
                });
            }
            catch (Exception caughtException)
            {
                exception = caughtException;
            }
            
            exception.Should().NotBeNull().And.BeOfType<GraphActionException>();
            exception!.Message.Should().Contain("there is no entity with id");
        }

        private const string CityLabel = "City";
        private const string DistanceLabel = "Distance";
        private const string HighwayLabel = "Highway";
        private const string NameLabel = "Name";
        private const string PopulationLabel = "Population";
        private const string CoordinatesLabel = "Coordinates";

        private static readonly Guid HamburgId = Guid.NewGuid();
        private static readonly Guid MunichId = Guid.NewGuid();
        private static readonly Guid BerlinId = Guid.NewGuid();
        private static readonly Guid A24Id = Guid.NewGuid();
        private static readonly Guid A9Id = Guid.NewGuid();
        private static readonly Guid A7Id = Guid.NewGuid();
        
        private static async Task<MemoryGraph> PrepareExampleGraph(string name)
        {
            var graph = new MemoryGraph(name);
            var hamburg = new Vertex(CityLabel, HamburgId)
                .WithAttribute(NameLabel, "Hamburg")
                .WithAttribute(PopulationLabel, 1841000)
                .WithAttribute(CoordinatesLabel, new Coordinate(53.55, 9.99));
            var berlin = new Vertex(CityLabel, BerlinId)
                .WithAttribute(NameLabel, "Berlin")
                .WithAttribute(PopulationLabel, 3645000)
                .WithAttribute(CoordinatesLabel, new Coordinate(52.52, 14.41));
            var munich = new Vertex(CityLabel, MunichId)
                .WithAttribute(NameLabel, "Munich")
                .WithAttribute(PopulationLabel, 1472000)
                .WithAttribute(CoordinatesLabel, new Coordinate(48.14, 11.58));
            var a24 = new Edge(HighwayLabel, hamburg.Id, berlin.Id, false, A24Id)
                .WithAttribute(NameLabel, "A24")
                .WithAttribute(DistanceLabel, 289.0);
            var a9 = new Edge(HighwayLabel, berlin.Id, munich.Id, false, A9Id)
                .WithAttribute(NameLabel, "A9")
                .WithAttribute(DistanceLabel, 585.0);
            var a7 = new Edge(HighwayLabel, munich.Id, hamburg.Id, false, A7Id)
                .WithAttribute(NameLabel, "A7")
                .WithAttribute(DistanceLabel, 778.0);

            await graph.Execute(new Transaction
            {
                hamburg.ToCreateVertexAction(),
                berlin.ToCreateVertexAction(),
                munich.ToCreateVertexAction(),
                a24.ToCreateEdgeAction(),
                a9.ToCreateEdgeAction(),
                a7.ToCreateEdgeAction()
            });

            return graph;
        }
    }
}
