using Graphene.InMemory;
using Graphene.Random;
using Xunit;

namespace Graphene.Test
{
    public class RandomGraphTest
    {
        [Fact]
        public void MinimalGraphShouldHaveOneVertex()
        {
            var graph = RandomGraphs.GetRandomGraph(1, 0.5d, () => new MemoryGraph(), RandomGraphs.edgeType.hybrid);
            Assert.Equal(1, graph.Size);
        }
    }
}
