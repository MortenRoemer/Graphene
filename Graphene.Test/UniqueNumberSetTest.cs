using Graphene.InMemory.Utility;
using Xunit;

namespace Graphene.Test
{
    public class UniqueNumberSetTest
    {
        [Fact]
        public void SampleRandomShouldGiveAnyNumberInRange()
        {
            var numbers = new UniqueNumberSet(10, 20);
            var iterationCount = 0;

            while(!numbers.IsEmpty)
            {
                var sample = numbers.SampleRandom();
                Assert.True(sample >= 10);
                Assert.True(sample <= 20);
                Assert.False(numbers.Contains(sample));
                iterationCount++;
            }

            Assert.Equal(11, iterationCount);
        }
    }
}