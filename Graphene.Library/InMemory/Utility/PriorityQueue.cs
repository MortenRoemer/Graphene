using System;
using System.Collections;
using System.Collections.Generic;

namespace Graphene.InMemory.Utility
{
    public class PriorityQueue<TPriority, TPayLoad> : IEnumerable<TPayLoad> where TPriority : IComparable<TPriority>
    {
        private const int STANDARD_CAPACITY = 10;

        private Entry[] Entries { get; set; } = new Entry[STANDARD_CAPACITY];

        public int Capacity => Entries.Length;

        public int Count { get; private set; }

        private int Beginning { get; set; }

        public bool IsEmpty => Count <= 0; 

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
                var target = new Entry[Capacity * 2];
                Array.Copy(Entries, Beginning, Entries, 0, Count);
                Beginning = 0;
                Entries = target;
            }
        }

        public void Insert(TPriority priority, TPayLoad payLoad)
        {
            if (priority is null)
                throw new ArgumentNullException(nameof(priority));
            
            if (payLoad is null)
                throw new ArgumentNullException(nameof(payLoad));
            
            RemoveExistingIfHigher(priority, payLoad);
            EnsureCapacity();

            for(var index = Beginning; index < Beginning + Count; index++)
            {
                var entry = Entries[index];

                if (priority.CompareTo(entry.Priority) >= 0)
                    continue;
                
                var blockSize = Count - index;
                Array.Copy(Entries, index, Entries,  index + 1, blockSize);
                Entries[index] = new Entry(priority, payLoad);
                Count++;
                return;
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

        private void RemoveExistingIfHigher(TPriority priority, TPayLoad payLoad)
        {
            for (var index = Beginning; index < Beginning + Count; index++)
            {
                var currentEntry = Entries[index];

                if (!currentEntry.PayLoad.Equals(payLoad))
                    continue;
                
                if (currentEntry.Priority.CompareTo(priority) <= 0)
                    return;
                    
                var blockSize = Count - index - 1;
                Array.Copy(Entries, index + 1, Entries, index, blockSize);
                Count--;
                return;
            }
        }

        public IEnumerator<TPayLoad> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private readonly struct Entry
        {
            public Entry(TPriority priority, TPayLoad payLoad)
            {
                Priority = priority;
                PayLoad = payLoad;
            }

            public TPriority Priority { get; }

            public TPayLoad PayLoad { get; }
        }

        private class Enumerator : IEnumerator<TPayLoad>
        {
            public Enumerator(PriorityQueue<TPriority, TPayLoad> backend)
            {
                Backend = backend ?? throw new ArgumentNullException(nameof(backend));
                Index = backend.Beginning - 1;
            }

            private PriorityQueue<TPriority, TPayLoad> Backend { get; set; }

            private int Index { get; set; }

            private bool Disposed { get; set; }

            public TPayLoad Current
            {
                get
                {
                    if (Disposed)
                        throw new ObjectDisposedException(nameof(Enumerator));

                    return Backend.Entries[Index].PayLoad;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                if (Disposed)
                    throw new ObjectDisposedException(nameof(Enumerator));

                Backend = null;
                Disposed = true;
            }

            public bool MoveNext()
            {
                if (Disposed)
                    throw new ObjectDisposedException(nameof(Enumerator));

                return ++Index < Backend.Beginning + Backend.Count;
            }

            public void Reset()
            {
                if (Disposed)
                    throw new ObjectDisposedException(nameof(Enumerator));

                Index = Backend.Beginning - 1;
            }
        }
    }
}