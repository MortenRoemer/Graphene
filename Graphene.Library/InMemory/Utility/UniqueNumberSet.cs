using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Graphene.InMemory.Utility
{
    public class UniqueNumberSet
    {
        public UniqueNumberSet()
        {
            Segments = new List<Segment>();
        }

        public UniqueNumberSet(ulong from, ulong to)
        {
            Segments = new List<Segment>
            {
                new Segment(from, to)
            };
        }

        private static readonly ThreadLocal<Random> RandomGenerator = new ThreadLocal<Random>(() => new Random(), trackAllValues: false);

        private static readonly ThreadLocal<byte[]> SampleBuffer = new ThreadLocal<byte[]>(() => new byte[8], trackAllValues: false);

        private readonly IList<Segment> Segments;

        public ulong Count => Segments.Aggregate(0uL, (sum, segment) => sum + segment.Count);

        public bool IsEmpty => Segments.Count <= 0;

        public void Add(ulong number)
        {
            if (FindSegment(number, out var index))
                return;

            if (index == 0)
            {
                if (Segments.Count <= 0)
                {
                    Segments.Add(new Segment(number, number));
                    return;
                }

                var segment = Segments[0];

                if (number == segment.Min - 1)
                    Segments[0] = new Segment(number, segment.Max);
                else 
                    Segments.Insert(0, new Segment(number, number));

                return;
            }

            if (index >= Segments.Count)
            {
                var segment = Segments[Segments.Count - 1];

                if (number == segment.Max + 1)
                    Segments[Segments.Count - 1] = new Segment(segment.Min, number);
                else
                    Segments.Add(new Segment(number, number));

                return;
            }

            var previosSegment = Segments[index - 1];
            var nextSegment = Segments[index];

            if (previosSegment.Max + 1 == number && nextSegment.Min - 1 == number)
            {
                Segments[index - 1] = new Segment(previosSegment.Min, nextSegment.Max);
                Segments.RemoveAt(index);
            }
            else if (previosSegment.Max + 1 == number)
            {
                Segments[index - 1] = new Segment(previosSegment.Min, number);
            }
            else if (nextSegment.Min - 1 == number)
            {
                Segments[index] = new Segment(number, nextSegment.Max);
            }
            else
            {
                Segments.Insert(index, new Segment(number, number));
            }
        }

        public bool Contains(ulong number)
        {
            return FindSegment(number, out _);
        }

        private bool FindSegment(ulong number, out int index)
        {
            var left = 0;
            var right = Segments.Count - 1;

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

        public void Remove(ulong number)
        {
            if (!FindSegment(number, out var index))
                return;

            var segment = Segments[index];

            if (!segment.Contains(number))
                throw new InvalidOperationException($"number {number} cannot be removed from segment [{segment.Min}-{segment.Max}]");

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
            var result = segment.GetRandom();
            Remove(result);
            return result;
        }

        private struct Segment
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

            public ulong Count => Max - Min + 1;

            public bool Contains(ulong number)
            {
                return Min <= number && number <= Max;
            }
            
            public ulong GetRandom()
            {
                var randomGenerator = RandomGenerator.Value;
                var buffer = SampleBuffer.Value;
                randomGenerator.NextBytes(buffer);
                var sample = (BitConverter.ToUInt64(buffer) % Count) + Min;
                return sample;
            }
        }
    }
}