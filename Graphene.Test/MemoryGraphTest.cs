using FluentAssertions;
using Graphene.InMemory;
using Graphene.Transactions;
using System;
using Xunit;

namespace Graphene.Test
{
    public class MemoryGraphTest
    {
        [Fact]
        public async void CreatingVerticesAndEdgesShouldWork()
        {
            var graph = new MemoryGraph(nameof(CreatingVerticesAndEdgesShouldWork));
            var mike = new MemoryVertex("Person");
            var samantha = new MemoryVertex("Person");
            var relationship = new MemoryEdge("Friendship", mike.Id, samantha.Id, false);
            
            await graph.Execute(new Transaction
            {
                mike.ToCreateVertexAction(),
                samantha.ToCreateVertexAction(),
                relationship.ToCreateEdgeAction()
            });

            var createdPersons = await graph.FindVertices(2, vertex => vertex.Label == "Person");
            createdPersons.Results.Should().HaveCount(2);

            var createdFriendships = await graph.FindEdges(1, edge => edge.Label == "Friendship");
            createdFriendships.Results.Should().HaveCount(1);
        }

        [Fact]
        public async void EmptyGuidsShouldBeRejected()
        {
            var graph = new MemoryGraph(nameof(EmptyGuidsShouldBeRejected));
            var mike = new MemoryVertex("Person", Guid.Empty);
            var relationship = new MemoryEdge("Love", mike.Id, mike.Id, false, Guid.Empty);
            Exception? exception = null;
            
            try
            {
                await graph.Execute(new Transaction
                {
                    mike.ToCreateVertexAction(),
                    relationship.ToCreateEdgeAction()
                });
            }
            catch (Exception caughtException)
            {
                exception = caughtException;
            }
            
            exception.Should().BeOfType<GraphActionException>();
            exception!.Message.Should().Contain("empty Guid is not valid");
        }

        [Fact]
        public async void UpdatingEntitiesShouldWork()
        {
            var graph = new MemoryGraph(nameof(UpdatingEntitiesShouldWork));
            var mike = new MemoryVertex("Person")
                .WithAttribute("Name", "Mike")
                .WithAttribute("Age", 18);
            var relationship = new MemoryEdge("Love", mike.Id, mike.Id, false)
                .WithAttribute("Intensity", "High")
                .WithAttribute("Awareness", 0);

            await graph.Execute(new Transaction
            {
                mike.ToCreateVertexAction(),
                relationship.ToCreateEdgeAction()
            });

            var mikePatch = mike.Patch()
                .WithAttribute("Age", 21);
            var relationshipPatch = relationship.Patch()
                .WithAttribute("Intensity", "Low");

            await graph.Execute(new Transaction
            {
                mikePatch.ToUpdateVertexAction(),
                relationshipPatch.ToUpdateEdgeAction()
            });

            var patchedMike = await graph.Get(mike.Id);
            patchedMike.Get<string>("Name").Should().Be("Mike");
            patchedMike.Get<int>("Age").Should().Be(21);

            var patchedRelationship = await graph.Get(relationship.Id);
            patchedRelationship.Get<string>("Intensity").Should().Be("Low");
            patchedRelationship.Get<int>("Awareness").Should().Be(0);
        }

        [Fact]
        public async void UpdatesToMissingEntitiesShouldFail()
        {
            var graph = new MemoryGraph(nameof(UpdatesToMissingEntitiesShouldFail));
            var misguidedPatch = new MemoryVertex("Person")
                .WithAttribute("Age", 18);
            Exception? exception = null;

            try
            {
                await graph.Execute(new Transaction
                {
                    misguidedPatch.ToUpdateVertexAction()
                });
            }
            catch (Exception caughtException)
            {
                exception = caughtException;
            }
            
            exception.Should().BeOfType<GraphActionException>();
            exception!.Message.Should().Contain("there is no vertex with id");
        }
    }
}
