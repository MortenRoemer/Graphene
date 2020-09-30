using System;

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
    }
}