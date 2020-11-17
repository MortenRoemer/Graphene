using System;
using System.Collections;
using System.Collections.Generic;

namespace Graphene.InMemory.Utility
{
    public class PriorityQueue<TPriority, TPayLoad> : IEnumerable<TPayLoad> where TPriority : IComparable<TPriority>
    {
        private const int STANDARD_CAPACITY = 10;

        private Memory<Entry> Entries { get; set; } = new Entry[STANDARD_CAPACITY];

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
                var source = Entries.Slice(Beginning, Count);
                var target = Entries.Slice(0, Count);
                source.CopyTo(target);
                Beginning = 0;
            }
            else
            {
                
                var source = Entries.Slice(Beginning, Count);
                Memory<Entry> target = new Entry[Capacity * 2];
                source.CopyTo(target);
                Beginning = 0;
                Entries = target;
            }
        }

        public void Insert(TPriority priority, TPayLoad payLoad)
        {
            RemoveExistingIfHigher(priority, payLoad);
            EnsureCapacity();
            var entries = Entries.Span;

            for(var index = Beginning; index < Beginning + Count; index++)
            {
                var entry = entries[index];

                if (priority.CompareTo(entry.Priority) >= 0)
                    continue;
                
                var blockSize = Count - index;
                var source = entries.Slice(index, blockSize);
                var target = entries.Slice(index + 1, blockSize);
                source.CopyTo(target);
                entries[index] = new Entry(priority, payLoad);
                Count++;
                return;
            }

            entries[Beginning + Count] = new Entry(priority, payLoad);
            Count++;
        }

        public TPayLoad PeekMin()
        {
            if (Count <= 0)
                throw new InvalidOperationException();

            return Entries.Span[Beginning].PayLoad;
        }

        public TPayLoad PeekMax()
        {
            if (Count <= 0)
                throw new InvalidOperationException();

            return Entries.Span[Beginning + Count - 1].PayLoad;
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
            var entries = Entries.Span;
            
            for (var index = Beginning; index < Beginning + Count; index++)
            {
                var currentEntry = entries[index];

                if (!currentEntry.PayLoad.Equals(payLoad))
                    continue;
                
                if (currentEntry.Priority.CompareTo(priority) <= 0)
                    return;
                    
                var blockSize = Count - index - 1;
                var source = entries.Slice(index + 1, blockSize);
                var target = entries.Slice(index, blockSize);
                source.CopyTo(target);
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

                    return Backend.Entries.Span[Index].PayLoad;
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