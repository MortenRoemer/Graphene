using System;
using System.Collections.Generic;

namespace Graphene.InMemory.Utility
{
    internal class UniqueNumberSet
    {
        public UniqueNumberSet(ulong from, ulong to)
        {
            RandomGenerator = new Lazy<Random>(isThreadSafe: false);
            SampleBuffer = new Lazy<byte[]>(() => new byte[8], isThreadSafe: false);
            Segments = new List<Segment>();
            Segments.Add(new Segment(from, to));
        }

        private Lazy<Random> RandomGenerator;

        private Lazy<byte[]> SampleBuffer;

        private List<Segment> Segments;

        public void Add(ulong number)
        {
            if (FindSegment(number, out var index))
                return;

            var previousSegment = Segments[index];
            var nextSegment = Segments[index + 1];

            if (previousSegment.Max == number - 1)
                Segments[index] = new Segment(previousSegment.Min, number);
            else if (nextSegment.Min == number + 1)
                Segments[index + 1] = new Segment(number, nextSegment.Max);
            else
                Segments.Insert(index, new Segment(number, number));
        }

        public bool Contains(ulong number)
        {
            return FindSegment(number, out _);
        }

        private bool FindSegment(ulong number, out int index)
        {
            var left = 0;
            var right = Segments.Count;

            while (left < right)
            {
                var middle = (right + left) / 2;
                var segment = Segments[middle];
                var comparison = segment.Compare(number);

                if (comparison < 0)
                    right = middle;
                else if (comparison > 0)
                    left = middle;
                else 
                {
                    index = middle;
                    return true;
                }
            }

            index = left;
            return false;
        }

        public void Remove(ulong number)
        {
            if (!FindSegment(number, out var index))
                return;

            var segment = Segments[index];

            if (number == segment.Min && number == segment.Max)
                Segments.RemoveAt(index);
            else if (number == segment.Min)
                Segments[index] = new Segment(segment.Min + 1, segment.Max);
            else if (number == segment.Max)
                Segments[index] = new Segment(segment.Min, segment.Max - 1);
            else
            {
                Segments[index] = new Segment(segment.Min, number - 1);
                Segments.Insert(index + 1, new Segment(number + 1, segment.Max));
            }
        }

        public ulong SampleRandom()
        {
            if (Segments.Count <= 0)
                throw new InvalidOperationException("no more numbers to sample");

            var randomGenerator = RandomGenerator.Value;
            var randomIndex = randomGenerator.Next(Segments.Count);
            var segment = Segments[randomIndex];

            if (segment.Min == segment.Max)
            {
                Segments.RemoveAt(randomIndex);
                return segment.Min;
            }

            var buffer = SampleBuffer.Value;
            randomGenerator.NextBytes(buffer);
            var sample = BitConverter.ToUInt64(buffer);
            var range = segment.Max - segment.Min;
            var result = (sample % range) + segment.Min;
            Remove(result);
            return result;
        }

        private struct Segment
        {
            public Segment(ulong first, ulong second)
            {
                Min = Math.Min(first, second);
                Max = Math.Max(first, second);
            }

            public ulong Min { get; }

            public ulong Max { get; }

            public int Compare(ulong number)
            {
                if (number < Min)
                    return -1;
                else if (number > Max)
                    return 1;
                else
                    return 0;
            }
        }
    }
}