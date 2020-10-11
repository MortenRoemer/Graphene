using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory.Query
{
    internal abstract class AttributeFilter : Filter
    {
        protected AttributeFilter(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        protected string Name { get; }

        internal class HasNoValue : AttributeFilter
        {
            public HasNoValue(string name) : base(name) {}

            public override bool Contains(IEntity entity)
            {
                return !entity.Attributes.TryGet<object>(Name, out _);
            }
        }

        internal class HasValue : AttributeFilter
        {
            public HasValue(string name) : base(name) {}

            public override bool Contains(IEntity entity)
            {
                return entity.Attributes.TryGet<object>(Name, out _);
            }
        }

        internal class IsBetween<T> : AttributeFilter
        {
            public IsBetween(string name, T from, T to) : base(name)
            {
                From = from ?? throw new ArgumentNullException(nameof(from));
                To = to ?? throw new ArgumentNullException(nameof(to));
            }

            private T From { get; }

            private T To { get; }

            public override bool Contains(IEntity entity)
            {
                if (!entity.Attributes.TryGet(Name, out IComparable<T> reference))
                    return false;

                return reference.CompareTo(From) >= 0 && reference.CompareTo(To) <= 0;
            }
        }

        internal class IsEqualTo<T> : AttributeFilter
        {
            public IsEqualTo(string name, T other) : base(name)
            {
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            private T Other { get; }

            public override bool Contains(IEntity entity)
            {
                return entity.Attributes.TryGet(Name, out T value) && value.Equals(Other);
            }
        }

        internal class IsGreaterOrEqualTo<T> : AttributeFilter
        {
            public IsGreaterOrEqualTo(string name, T other) : base(name)
            {
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            private T Other { get; }

            public override bool Contains(IEntity entity)
            {
                return entity.Attributes.TryGet(Name, out IComparable<T> value) && value.CompareTo(Other) >= 0;
            }
        }

        internal class IsGreaterThan<T> : AttributeFilter
        {
            public IsGreaterThan(string name, T other) : base(name)
            {
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            private T Other { get; }

            public override bool Contains(IEntity entity)
            {
                return entity.Attributes.TryGet(Name, out IComparable<T> value) && value.CompareTo(Other) > 0;
            }
        }

        internal class IsIn<T> : AttributeFilter
        {
            public IsIn(string name, IEnumerable<T> range) : base(name)
            {
                Range = range ?? throw new ArgumentNullException(nameof(range));
            }

            private IEnumerable<T> Range { get; }

            public override bool Contains(IEntity entity)
            {
                return entity.Attributes.TryGet(Name, out T value) && Range.Contains(value);
            }
        }

        internal class IsLessOrEqualTo<T> : AttributeFilter
        {
            public IsLessOrEqualTo(string name, T other) : base(name)
            {
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            private T Other { get; }

            public override bool Contains(IEntity entity)
            {
                return entity.Attributes.TryGet(Name, out IComparable<T> value) && value.CompareTo(Other) <= 0;
            }
        }

        internal class IsLessThan<T> : AttributeFilter
        {
            public IsLessThan(string name, T other) : base(name)
            {
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            private T Other { get; }

            public override bool Contains(IEntity entity)
            {
                return entity.Attributes.TryGet(Name, out IComparable<T> value) && value.CompareTo(Other) < 0;
            }
        }

        internal class IsNotBetween<T> : AttributeFilter
        {
            public IsNotBetween(string name, T from, T to) : base(name)
            {
                From = from ?? throw new ArgumentNullException(nameof(from));
                To = to ?? throw new ArgumentNullException(nameof(to));
            }

            private T From { get; }

            private T To { get; }

            public override bool Contains(IEntity entity)
            {
                if (!entity.Attributes.TryGet(Name, out IComparable<T> reference))
                    return true;

                return reference.CompareTo(From) < 0 || reference.CompareTo(To) > 0;
            }
        }

        internal class IsNotEqualTo<T> : AttributeFilter
        {
            public IsNotEqualTo(string name, T other) : base(name)
            {
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            private T Other { get; }

            public override bool Contains(IEntity entity)
            {
                return !entity.Attributes.TryGet(Name, out object value) || !value.Equals(Other);
            }
        }

        internal class IsNotIn<T> : AttributeFilter
        {
            public IsNotIn(string name, IEnumerable<T> range) : base(name)
            {
                Range = range ?? throw new ArgumentNullException(nameof(range));
            }

            private IEnumerable<T> Range { get; }

            public override bool Contains(IEntity entity)
            {
                return !entity.Attributes.TryGet(Name, out T value) || !Range.Contains(value);
            }
        }
    }
}