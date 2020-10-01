using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Graphene.InMemory.Query
{
    internal abstract class LabelFilter : Filter
    {
        private static readonly StringComparer Comparer = StringComparer.Ordinal;

        public class DoesMatch : LabelFilter
        {
            public DoesMatch(string pattern)
            {
                Pattern = new Regex(pattern);
            }

            private Regex Pattern { get; }

            public override bool Contains(MemoryQueryAgent agent, IEntity entity)
            {
                return Pattern.IsMatch(entity.Label);
            }
        }

        public class DoesNotMatch : LabelFilter
        {
            public DoesNotMatch(string pattern)
            {
                Pattern = new Regex(pattern);
            }

            private Regex Pattern { get; }

            public override bool Contains(MemoryQueryAgent agent, IEntity entity)
            {
                return !Pattern.IsMatch(entity.Label);
            }
        }

        public class IsEqualTo : LabelFilter
        {
            public IsEqualTo(string other)
            {
                Other = other;
            }

            private string Other { get; }

            public override bool Contains(MemoryQueryAgent agent, IEntity entity)
            {
                return Comparer.Equals(entity.Label, Other);
            }
        }

        public class IsIn : LabelFilter
        {
            public IsIn(IEnumerable<string> range)
            {
                Range = range ?? throw new ArgumentNullException(nameof(range));
            }

            private IEnumerable<string> Range { get; }

            public override bool Contains(MemoryQueryAgent agent, IEntity entity)
            {
                foreach (var other in Range)
                {
                    if (Comparer.Equals(entity.Label, other))
                        return true;
                }

                return false;
            }
        }

        public class IsNotEqualTo : LabelFilter
        {
            public IsNotEqualTo(string other)
            {
                Other = other;
            }

            private string Other { get; }

            public override bool Contains(MemoryQueryAgent agent, IEntity entity)
            {
                return !Comparer.Equals(entity.Label, Other);
            }
        }

        public class IsNotIn : LabelFilter
        {
            public IsNotIn(IEnumerable<string> range)
            {
                Range = range ?? throw new ArgumentNullException(nameof(range));
            }

            private IEnumerable<string> Range { get; }

            public override bool Contains(MemoryQueryAgent agent, IEntity entity)
            {
                foreach (var other in Range)
                {
                    if (Comparer.Equals(entity.Label, other))
                        return false;
                }

                return true;
            }
        }
    }
}