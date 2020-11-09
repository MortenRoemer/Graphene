using System;
using System.Threading;

namespace Graphene.InMemory.Utility
{
    public class UniqueNumberSet
    {
        private const int STANDARD_CAPACITY = 10;
        
        public UniqueNumberSet()
        {
            Segments = new Segment[STANDARD_CAPACITY];
        }

        public UniqueNumberSet(ulong from, ulong to) : this()
        {
            AppendSegment(new Segment(from, to));
        }

        private static readonly ThreadLocal<Random> RandomGenerator = new ThreadLocal<Random>(() => new Random(), trackAllValues: false);

        private Memory<Segment> Segments { get; set; }
        
        private int SegmentCount { get; set; }

        private int Capacity => Segments.Length;

        public bool IsEmpty => SegmentCount <= 0;

        public void Add(ulong number)
        {
            if (FindSegment(number, out var index))
                return;

            var segments = Segments.Span;

            if (index == 0)
            {
                if (SegmentCount <= 0)
                {
                    AppendSegment(new Segment(number, number));
                    return;
                }

                var segment = segments[0];

                if (number == segment.Min - 1)
                    segments[0] = new Segment(number, segment.Max);
                else 
                    InsertSegment(0, new Segment(number, number));

                return;
            }

            if (index >= SegmentCount)
            {
                var segment = segments[^1];

                if (number == segment.Max + 1)
                    segments[^1] = new Segment(segment.Min, number);
                else
                    AppendSegment(new Segment(number, number));

                return;
            }

            var previousSegment = segments[index - 1];
            var nextSegment = segments[index];

            if (previousSegment.Max + 1 == number && nextSegment.Min - 1 == number)
            {
                segments[index - 1] = new Segment(previousSegment.Min, nextSegment.Max);
                RemoveSegment(index);
            }
            else if (previousSegment.Max + 1 == number)
            {
                segments[index - 1] = new Segment(previousSegment.Min, number);
            }
            else if (nextSegment.Min - 1 == number)
            {
                segments[index] = new Segment(number, nextSegment.Max);
            }
            else
            {
                InsertSegment(index, new Segment(number, number));
            }
        }

        public bool Contains(ulong number)
        {
            return FindSegment(number, out _);
        }

        private bool FindSegment(ulong number, out int index)
        {
            var left = 0;
            var right = SegmentCount - 1;
            var segments = Segments.Span;

            while (left <= right)
            {
                var middle = left + (right - left) / 2;
                var segment = segments[middle];

                if (segment.Contains(number))
                {
                    index = middle;
                    return true;
                }
                else if (number < segment.Min)
                    right = middle - 1;
                else
                    left = middle + 1;
            }

            index = left;
            return false;
        }

        public void Remove(ulong number)
        {
            if (!FindSegment(number, out var index))
                return;

            var segments = Segments.Span;
            var segment = segments[index];

            if (!segment.Contains(number))
                throw new InvalidOperationException($"number {number} cannot be removed from segment [{segment.Min}-{segment.Max}]");

            if (number == segment.Min && number == segment.Max)
                RemoveSegment(index);
            else if (number == segment.Min)
                segments[index] = new Segment(segment.Min + 1, segment.Max);
            else if (number == segment.Max)
                segments[index] = new Segment(segment.Min, segment.Max - 1);
            else
            {
                segments[index] = new Segment(segment.Min, number - 1);
                InsertSegment(index + 1, new Segment(number + 1, segment.Max));
            }
        }

        public ulong SampleRandom()
        {
            if (SegmentCount <= 0)
                throw new InvalidOperationException("no more numbers to sample");
            
            var randomGenerator = RandomGenerator.Value;
            var randomIndex = randomGenerator.Next(SegmentCount);
            var segment = Segments.Span[randomIndex];
            var result = segment.GetRandom();
            Remove(result);
            return result;
        }

        private void AppendSegment(Segment segment)
        {
            EnsureCapacity();
            Segments.Span[SegmentCount++] = segment;
        }

        private void InsertSegment(int index, Segment segment)
        {
            EnsureCapacity();
            var blockSize = SegmentCount - index;
            var source = Segments.Slice(index, blockSize);
            var target = Segments.Slice(index + 1);
            source.CopyTo(target);
            Segments.Span[index] = segment;
            SegmentCount++;
        }

        private void RemoveSegment(int index)
        {
            var blockSize = SegmentCount - index - 1;
            var source = Segments.Slice(index + 1, blockSize);
            var target = Segments.Slice(index);
            source.CopyTo(target);
            SegmentCount--;
        }

        private void EnsureCapacity()
        {
            if (Capacity > SegmentCount)
                return;

            var source = Segments.Slice(0, SegmentCount);
            Memory<Segment> target = new Segment[Capacity * 2];
            source.CopyTo(target);
            Segments = target;
        }

        private readonly struct Segment
        {
            public Segment(ulong min, ulong max)
            {
                if (min > max)
                    throw new ArgumentException($"segment min {min} is greater than {max}");

                Min = min;
                Max = max;
            }

            public ulong Min { get; }

            public ulong Max { get; }

            private ulong Count => Max - Min + 1;

            public bool Contains(ulong number)
            {
                return Min <= number && number <= Max;
            }

            public unsafe ulong GetRandom()
            {
                var randomGenerator = RandomGenerator.Value;
                Span<byte> buffer = stackalloc byte[8];
                randomGenerator.NextBytes(buffer);
                var sample = (BitConverter.ToUInt64(buffer) % Count) + Min;
                return sample;
            }
        }
    }
}