using Graphene.InMemory.Utility;
using Xunit;

namespace Graphene.Test
{
    public class PriorityQueueTest
    {
        [Fact]
        public void InsertingEntriesShouldOrderByPriority()
        {
            var queue = new PriorityQueue<byte, char>();
            queue.Insert(4, 'a');
            queue.Insert(1, 'b');
            queue.Insert(2, 'c');
            
            using var entries = queue.GetEnumerator();
            Assert.True(entries.MoveNext());
            Assert.Equal('b', entries.Current);
            Assert.True(entries.MoveNext());
            Assert.Equal('c', entries.Current);
            Assert.True(entries.MoveNext());
            Assert.Equal('a', entries.Current);
            Assert.False(entries.MoveNext());
        }

        [Fact]
        public void PeekAndRemoveMinShouldGiveAlwaysTheLowestPriority()
        {
            var queue = new PriorityQueue<byte, char>();
            queue.Insert(4, 'a');
            queue.Insert(1, 'b');
            queue.Insert(2, 'c');
            
            Assert.Equal('b', queue.PeekMin());
            Assert.Equal('b', queue.RemoveMin());
            Assert.Equal('c', queue.PeekMin());
            Assert.Equal('c', queue.RemoveMin());
            Assert.Equal('a', queue.PeekMin());
            Assert.Equal('a', queue.RemoveMin());
            Assert.Equal(0, queue.Count);
        }

        [Fact]
        public void PeekAndRemoveMaxShouldGiveAlwaysTheHighestPriority()
        {
            var queue = new PriorityQueue<byte, char>();
            queue.Insert(4, 'a');
            queue.Insert(1, 'b');
            queue.Insert(2, 'c');
            
            Assert.Equal('a', queue.PeekMax());
            Assert.Equal('a', queue.RemoveMax());
            Assert.Equal('c', queue.PeekMax());
            Assert.Equal('c', queue.RemoveMax());
            Assert.Equal('b', queue.PeekMax());
            Assert.Equal('b', queue.RemoveMax());
            Assert.Equal(0, queue.Count);
        }

        [Fact]
        public void CapacityGrowthShouldWorkCorrectly()
        {
            var queue = new PriorityQueue<int, char>();
            var startCapacity = queue.Capacity;

            while (queue.Capacity == startCapacity)
            {
                queue.Insert(queue.Count, queue.Count % 2 == 0 ? 'a' : 'A');
            }
            
            Assert.True(queue.Capacity > startCapacity);
        }
    }
}