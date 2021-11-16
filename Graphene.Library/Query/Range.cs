using System;

namespace Graphene.Query
{
    public readonly struct Range<T> where T : IComparable<T>
    {
        public Range(T from, T to)
        {
            if (from.CompareTo(to) <= 0)
            {
                Min = from;
                Max = to;
            }
            else
            {
                Min = to;
                Max = from;
            }
        }

        public T Min { get; }
        
        public T Max { get; }

        public bool Contains(T value)
        {
            return Min.CompareTo(value) <= 0 && Max.CompareTo(value) >= 0;
        }
    }
}