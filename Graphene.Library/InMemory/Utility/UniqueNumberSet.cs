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

        public UniqueNumberSet(int from, int to) : this()
        {
            AppendSegment(new Segment(from, to));
        }

        private static readonly ThreadLocal<Random> _randomGenerator = new ThreadLocal<Random>(() => new Random(), trackAllValues: false);

        private Segment[] Segments { get; set; }
        
        private int SegmentCount { get; set; }

        private int Capacity => Segments.Length;

        public bool IsEmpty => SegmentCount <= 0;

        public void Add(int number)
        {
            if (FindSegment(number, out var index))
                return;

            if (index == 0)
            {
                if (SegmentCount <= 0)
                {
                    AppendSegment(new Segment(number, number));
                    return;
                }

                var segment = Segments[0];

                if (number == segment.Min - 1)
                    Segments[0] = new Segment(number, segment.Max);
                else 
                    InsertSegment(0, new Segment(number, number));

                return;
            }

            if (index >= SegmentCount)
            {
                var segment = Segments[Segments.Length - 1];

                if (number == segment.Max + 1)
                    Segments[Segments.Length - 1] = new Segment(segment.Min, number);
                else
                    AppendSegment(new Segment(number, number));

                return;
            }

            var previousSegment = Segments[index - 1];
            var nextSegment = Segments[index];

            if (previousSegment.Max + 1 == number && nextSegment.Min - 1 == number)
            {
                Segments[index - 1] = new Segment(previousSegment.Min, nextSegment.Max);
                RemoveSegment(index);
            }
            else if (previousSegment.Max + 1 == number)
            {
                Segments[index - 1] = new Segment(previousSegment.Min, number);
            }
            else if (nextSegment.Min - 1 == number)
            {
                Segments[index] = new Segment(number, nextSegment.Max);
            }
            else
            {
                InsertSegment(index, new Segment(number, number));
            }
        }

        public bool Contains(int number)
        {
            return FindSegment(number, out _);
        }

        private bool FindSegment(int number, out int index)
        {
            var left = 0;
            var right = SegmentCount - 1;

            while (left <= right)
            {
                var middle = left + (right - left) / 2;
                var segment = Segments[middle];

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

        public void Remove(int number)
        {
            if (!FindSegment(number, out var index))
                return;
            
            var segment = Segments[index];

            if (!segment.Contains(number))
                throw new InvalidOperationException($"number {number} cannot be removed from segment [{segment.Min}-{segment.Max}]");

            if (number == segment.Min && number == segment.Max)
                RemoveSegment(index);
            else if (number == segment.Min)
                Segments[index] = new Segment(segment.Min + 1, segment.Max);
            else if (number == segment.Max)
                Segments[index] = new Segment(segment.Min, segment.Max - 1);
            else
            {
                Segments[index] = new Segment(segment.Min, number - 1);
                InsertSegment(index + 1, new Segment(number + 1, segment.Max));
            }
        }

        public int SampleRandom()
        {
            if (SegmentCount <= 0)
                throw new InvalidOperationException("no more numbers to sample");
            
            var randomGenerator = _randomGenerator.Value;
            var randomIndex = randomGenerator!.Next(SegmentCount);
            var segment = Segments[randomIndex];
            var result = segment.GetRandom();
            Remove(result);
            return result;
        }

        private void AppendSegment(Segment segment)
        {
            EnsureCapacity();
            Segments[SegmentCount++] = segment;
        }

        private void InsertSegment(int index, Segment segment)
        {
            EnsureCapacity();
            var blockSize = SegmentCount - index;
            Array.Copy(Segments, index, Segments, index + 1, blockSize);
            Segments[index] = segment;
            SegmentCount++;
        }

        private void RemoveSegment(int index)
        {
            var blockSize = SegmentCount - index - 1;
            Array.Copy(Segments, index + 1, Segments, index, blockSize);
            SegmentCount--;
        }

        private void EnsureCapacity()
        {
            if (Capacity > SegmentCount)
                return;
            
            var newSegments = new Segment[Capacity * 2];
            Array.Copy(Segments, 0, newSegments, 0, SegmentCount);
            Segments = newSegments;
        }

        private readonly struct Segment
        {
            public Segment(int min, int max)
            {
                if (min > max)
                    throw new ArgumentException($"segment min {min} is greater than {max}");

                Min = min;
                Max = max;
            }

            public int Min { get; }

            public int Max { get; }

            public bool Contains(int number)
            {
                return Min <= number && number <= Max;
            }

            public int GetRandom()
            {
                var randomGenerator = _randomGenerator.Value;
                return randomGenerator!.Next(Min - 1, Max) + 1;
            }
        }
    }
}