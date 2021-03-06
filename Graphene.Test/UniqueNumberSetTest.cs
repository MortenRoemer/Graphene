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

        [Fact]
        public void NumberRemovalShouldWork()
        {
            var numbers = new UniqueNumberSet(10, 20);

            numbers.Remove(15);
            numbers.Remove(19);
            numbers.Remove(10);
            numbers.Remove(14);
            Assert.False(numbers.Contains(10));
            Assert.True(numbers.Contains(11));
            Assert.True(numbers.Contains(12));
            Assert.True(numbers.Contains(13));
            Assert.False(numbers.Contains(14));
            Assert.False(numbers.Contains(15));
            Assert.True(numbers.Contains(16));
            Assert.True(numbers.Contains(17));
            Assert.True(numbers.Contains(18));
            Assert.False(numbers.Contains(19));
            Assert.True(numbers.Contains(20));
        }

        [Fact]
        public void NumberAdditionShouldWork()
        {
            var numbers = new UniqueNumberSet();
            numbers.Add(3);
            numbers.Add(1);
            numbers.Add(4);
            numbers.Add(2);
            numbers.Add(5);
            Assert.False(numbers.Contains(0));
            Assert.True(numbers.Contains(1));
            Assert.True(numbers.Contains(2));
            Assert.True(numbers.Contains(3));
            Assert.True(numbers.Contains(4));
            Assert.True(numbers.Contains(5));
            Assert.False(numbers.Contains(6));
        }
    }
}