using FluentAssertions;
using Graphene.InMemory;
using Graphene.Transactions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Graphene.Test
{
    public class MemoryGraphTest
    {
        [Fact]
        public async void CreatingVerticesAndEdgesShouldWork()
        {
            var graph = await PrepareExampleGraph(nameof(CreatingVerticesAndEdgesShouldWork));
            var bremen = new MemoryVertex(CityLabel);
            var a1 = new MemoryEdge(HighwayLabel, bremen.Id, HamburgId, false);

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
            var bremen = new MemoryVertex(CityLabel, Guid.Empty);
            var a1 = new MemoryEdge(HighwayLabel, bremen.Id, HamburgId, false, Guid.Empty);
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
            var anotherHamburg = new MemoryVertex(CityLabel, HamburgId);
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
            var hamburgPatch = new MemoryVertex(CityLabel, HamburgId)
                .WithAttribute(PopulationLabel, 2000000)
                .WithAttribute("Metropolis", true);
            var a24Patch = new MemoryEdge(HighwayLabel, HamburgId, BerlinId, false, a24Id)
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

            var patchedA24 = await graph.Get(a24Id);
            patchedA24.Get<string>(NameLabel).Should().Be("A24");
            patchedA24.Get<double>(DistanceLabel).Should().Be(300.0);
            patchedA24.Get<bool>("UnderConstruction").Should().Be(true);
        }

        [Fact]
        public async void UpdatesToMissingEntitiesShouldFail()
        {
            var graph = await PrepareExampleGraph(nameof(UpdatesToMissingEntitiesShouldFail));
            var misguidedPatch = new MemoryVertex(CityLabel);
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
            var hamburg = new MemoryVertex(CityLabel, HamburgId);

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
        public async void DeletionsOfMissingEntitiesShouldFail()
        {
            var graph = await PrepareExampleGraph(nameof(UpdatesToMissingEntitiesShouldFail));
            var misguidedDeletion = new MemoryVertex(CityLabel);
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

        private static readonly Guid HamburgId = Guid.NewGuid();
        private static readonly Guid MunichId = Guid.NewGuid();
        private static readonly Guid BerlinId = Guid.NewGuid();
        private static readonly Guid a24Id = Guid.NewGuid();
        private static readonly Guid a9Id = Guid.NewGuid();
        private static readonly Guid a7Id = Guid.NewGuid();
        
        private static async Task<MemoryGraph> PrepareExampleGraph(string name)
        {
            var graph = new MemoryGraph(name);
            var hamburg = new MemoryVertex(CityLabel, HamburgId)
                .WithAttribute(NameLabel, "Hamburg")
                .WithAttribute(PopulationLabel, 1841000);
            var berlin = new MemoryVertex(CityLabel, BerlinId)
                .WithAttribute(NameLabel, "Berlin")
                .WithAttribute(PopulationLabel, 3645000);
            var munich = new MemoryVertex(CityLabel, MunichId)
                .WithAttribute(NameLabel, "Munich")
                .WithAttribute(PopulationLabel, 1472000);
            var a24 = new MemoryEdge(HighwayLabel, hamburg.Id, berlin.Id, false, a24Id)
                .WithAttribute(NameLabel, "A24")
                .WithAttribute(DistanceLabel, 289.0);
            var a9 = new MemoryEdge(HighwayLabel, berlin.Id, munich.Id, false, a9Id)
                .WithAttribute(NameLabel, "A9")
                .WithAttribute(DistanceLabel, 585.0);
            var a7 = new MemoryEdge(HighwayLabel, munich.Id, hamburg.Id, false, a7Id)
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
