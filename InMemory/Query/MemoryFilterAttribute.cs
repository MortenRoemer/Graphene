using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryFilterAttribute<T> : IFilterAttribute<T>
    {
        internal MemoryFilterAttribute(MemoryFilterRoot<T> root, string name)
        {
            Root = root;
            Name = name;
        }

        private MemoryFilterRoot<T> Root { get; }

        private string Name { get; }

        public IFilterSequence<T> HasNoValue()
        {
            Root.AddFilter(new AttributeFilter.HasNoValue(Name));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> HasValue()
        {
            Root.AddFilter(new AttributeFilter.HasValue(Name));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsBetween<V>(V from, V to)
        {
            Root.AddFilter(new AttributeFilter.IsBetween<V>(Name, from, to));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsEqualTo<V>(V other)
        {
            Root.AddFilter(new AttributeFilter.IsEqualTo<V>(Name, other));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsGreaterOrEqualTo<V>(V other)
        {
            Root.AddFilter(new AttributeFilter.IsGreaterOrEqualTo<V>(Name, other));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsGreaterThan<V>(V other)
        {
            Root.AddFilter(new AttributeFilter.IsGreaterThan<V>(Name, other));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsIn<V>(IEnumerable<V> values)
        {
            Root.AddFilter(new AttributeFilter.IsIn<V>(Name, values));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsLessOrEqualTo<V>(V other)
        {
            Root.AddFilter(new AttributeFilter.IsLessOrEqualTo<V>(Name, other));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsLessThan<V>(V other)
        {
            Root.AddFilter(new AttributeFilter.IsLessThan<V>(Name, other));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsNotBetween<V>(V from, V to)
        {
            Root.AddFilter(new AttributeFilter.IsNotBetween<V>(Name, from, to));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsNotEqualTo<V>(V other)
        {
            Root.AddFilter(new AttributeFilter.IsNotEqualTo<V>(Name, other));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsNotIn<V>(IEnumerable<V> values)
        {
            Root.AddFilter(new AttributeFilter.IsNotIn<V>(Name, values));
            return new MemoryFilterSequence<T>(Root);
        }
    }
}