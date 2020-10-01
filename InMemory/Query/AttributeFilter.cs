using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory.Query
{
    internal abstract class AttributeFilter
    {
        protected AttributeFilter(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        protected string Name { get; }

        public abstract bool Contains(IGraph graph, IEntity entity);

        internal class HasNoValue : AttributeFilter
        {
            public HasNoValue(string name) : base(name) {}

            public override bool Contains(IGraph graph, IEntity entity)
            {
                return !entity.Attributes.TryGet<object>(Name, out _);
            }
        }

        internal class HasValue : AttributeFilter
        {
            public HasValue(string name) : base(name) {}

            public override bool Contains(IGraph graph, IEntity entity)
            {
                return entity.Attributes.TryGet<object>(Name, out _);
            }
        }

        internal class IsBetween : AttributeFilter
        {
            public IsBetween(string name, object from, object to) : base(name)
            {
                From = from ?? throw new ArgumentNullException(nameof(from));
                To = to ?? throw new ArgumentNullException(nameof(to));
            }

            private object From { get; }

            private object To { get; }

            public override bool Contains(IGraph graph, IEntity entity)
            {
                if (!entity.Attributes.TryGet(Name, out IComparable reference))
                    return false;

                return reference.CompareTo(From) >= 0 && reference.CompareTo(To) <= 0;
            }
        }

        internal class IsEqualTo : AttributeFilter
        {
            public IsEqualTo(string name, object other) : base(name)
            {
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            private object Other { get; }

            public override bool Contains(IGraph graph, IEntity entity)
            {
                return entity.Attributes.TryGet(Name, out object value) && value.Equals(Other);
            }
        }

        internal class IsGreaterOrEqualTo : AttributeFilter
        {
            public IsGreaterOrEqualTo(string name, object other) : base(name)
            {
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            private object Other { get; }

            public override bool Contains(IGraph graph, IEntity entity)
            {
                return entity.Attributes.TryGet(Name, out IComparable value) && value.CompareTo(Other) >= 0;
            }
        }

        internal class IsGreaterThan : AttributeFilter
        {
            public IsGreaterThan(string name, object other) : base(name)
            {
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            private object Other { get; }

            public override bool Contains(IGraph graph, IEntity entity)
            {
                return entity.Attributes.TryGet(Name, out IComparable value) && value.CompareTo(Other) > 0;
            }
        }

        internal class IsIn : AttributeFilter
        {
            public IsIn(string name, IEnumerable<object> range) : base(name)
            {
                Range = range ?? throw new ArgumentNullException(nameof(range));
            }

            private IEnumerable<object> Range { get; }

            public override bool Contains(IGraph graph, IEntity entity)
            {
                return entity.Attributes.TryGet(Name, out object value) && Range.Contains(value);
            }
        }

        internal class IsLessOrEqualTo : AttributeFilter
        {
            public IsLessOrEqualTo(string name, object other) : base(name)
            {
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            private object Other { get; }

            public override bool Contains(IGraph graph, IEntity entity)
            {
                return entity.Attributes.TryGet(Name, out IComparable value) && value.CompareTo(Other) <= 0;
            }
        }

        internal class IsLessThan : AttributeFilter
        {
            public IsLessThan(string name, object other) : base(name)
            {
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            private object Other { get; }

            public override bool Contains(IGraph graph, IEntity entity)
            {
                return entity.Attributes.TryGet(Name, out IComparable value) && value.CompareTo(Other) < 0;
            }
        }

        internal class IsNotBetween : AttributeFilter
        {
            public IsNotBetween(string name, object from, object to) : base(name)
            {
                From = from ?? throw new ArgumentNullException(nameof(from));
                To = to ?? throw new ArgumentNullException(nameof(to));
            }

            private object From { get; }

            private object To { get; }

            public override bool Contains(IGraph graph, IEntity entity)
            {
                if (!entity.Attributes.TryGet(Name, out IComparable reference))
                    return true;

                return reference.CompareTo(From) < 0 || reference.CompareTo(To) > 0;
            }
        }

        internal class IsNotEqualTo : AttributeFilter
        {
            public IsNotEqualTo(string name, object other) : base(name)
            {
                Other = other ?? throw new ArgumentNullException(nameof(other));
            }

            private object Other { get; }

            public override bool Contains(IGraph graph, IEntity entity)
            {
                return !entity.Attributes.TryGet(Name, out object value) || !value.Equals(Other);
            }
        }

        internal class IsNotIn : AttributeFilter
        {
            public IsNotIn(string name, IEnumerable<object> range) : base(name)
            {
                Range = range ?? throw new ArgumentNullException(nameof(range));
            }

            private IEnumerable<object> Range { get; }

            public override bool Contains(IGraph graph, IEntity entity)
            {
                return !entity.Attributes.TryGet(Name, out object value) || !Range.Contains(value);
            }
        }
    }
}