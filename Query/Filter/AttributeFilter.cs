using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.Query.Filter
{
    public abstract class AttributeFilter : EntityFilter
    {
        protected AttributeFilter(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public class Equal : AttributeFilter
        {
            public Equal(string name, object value) : base(name)
            {
                Value = value;
            }

            public object Value { get; }

            public override bool Contains(IEntity entity) =>
                entity.Attributes.TryGet(Name, out object otherValue) ? Value.Equals(otherValue) : false;
        }

        public class GreaterThan<T> : AttributeFilter where T : IComparable<T>
        {
            public GreaterThan(string name, T value) : base(name)
            {
                Value = value;
            }

            public T Value { get; }

            public override bool Contains(IEntity entity) =>
                entity.Attributes.TryGet(Name, out T otherValue) ? Value.CompareTo(otherValue) > 0 : false;
        }

        public class LessThan<T> : AttributeFilter where T : IComparable<T>
        {
            public LessThan(string name, T value) : base(name)
            {
                Value = value;
            }

            public T Value { get; }

            public override bool Contains(IEntity entity) =>
                entity.Attributes.TryGet(Name, out T otherValue) ? Value.CompareTo(otherValue) < 0 : false;
        }

        public class HasValue : AttributeFilter
        {
            public HasValue(string name) : base(name)
            {
            }

            public override bool Contains(IEntity entity) =>
                entity.Attributes.TryGet<object>(Name, out _);
        }

        public class In : AttributeFilter
        {
            public In(string name, IEnumerable<object> values) : base(name)
            {
                Values = values ?? throw new ArgumentNullException(nameof(values));
            }

            public IEnumerable<object> Values { get; }

            public override bool Contains(IEntity entity) =>
                entity.Attributes.TryGet(Name, out object otherValue) ? Values.Contains(otherValue) : false;
        }

        public class Between<T> : AttributeFilter where T : IComparable<T>
        {
            public Between(string name, T from, T to) : base(name)
            {
                FromValue = from;
                ToValue = to;
            }

            public T FromValue { get; }

            public T ToValue { get; }

            public override bool Contains(IEntity entity) =>
                entity.Attributes.TryGet(Name, out T otherValue)
                    ? (otherValue.CompareTo(FromValue) >= 0) && (otherValue.CompareTo(ToValue) <= 0)
                    : false;
        }
    }
}