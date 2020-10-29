using System;

namespace Graphene.InMemory.Utility
{
    public class PriorityQueue<TPriority, TPayLoad> where TPriority : IComparable<TPriority>
    {
        private const int STANDARD_CAPACITY = 10;

        private Entry[] Entries = new Entry[STANDARD_CAPACITY];

        public int Capacity => Entries.GetLength(0);

        public int Count { get; private set; } = 0;

        private int Beginning { get; set; } = 0;

        public void Clear()
        {
            Beginning = 0;
            Count = 0;
        }

        private void EnsureCapacity()
        {
            if (Count + Beginning < Capacity - 1)
                return;

            if (Beginning > 0)
            {
                Array.Copy(Entries, Beginning, Entries, 0, Count);
                Beginning = 0;
            }
            else
            {
                var newEntries = new Entry[Capacity * 2];
                Array.Copy(Entries, Beginning, newEntries, 0, Count);
                Beginning = 0;
                Entries = newEntries;
            }
        }

        public void Insert(TPriority priority, TPayLoad payLoad)
        {
            EnsureCapacity();

            for(int index = Beginning; index < Beginning + Count; index++)
            {
                var entry = Entries[index];

                if (priority.CompareTo(entry.Priority) > 0)
                {
                    Array.Copy(Entries, index, Entries, index + 1, Count - (index - Beginning));
                    Entries[index] = new Entry(priority, payLoad);
                    Count++;
                    return;
                }
            }

            Entries[Beginning + Count] = new Entry(priority, payLoad);
            Count++;
        }

        public TPayLoad PeekMin()
        {
            if (Count <= 0)
                throw new InvalidOperationException();

            return Entries[Beginning].PayLoad;
        }

        public TPayLoad PeekMax()
        {
            if (Count <= 0)
                throw new InvalidOperationException();

            return Entries[Beginning + Count - 1].PayLoad;
        }

        public TPayLoad RemoveMin()
        {
            var result = PeekMin();
            Beginning++;
            Count--;
            return result;
        }

        public TPayLoad RemoveMax()
        {
            var result = PeekMax();
            Count--;
            return result;
        }

        private struct Entry
        {
            public Entry(TPriority priority, TPayLoad payLoad)
            {
                Priority = priority;
                PayLoad = payLoad;
            }

            public TPriority Priority { get; }

            public TPayLoad PayLoad { get; }
        }
    }
}