using System;
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

        public IFilterSequence<T> IsBetween(object from, object to)
        {
            Root.AddFilter(new AttributeFilter.IsBetween(Name, from, to));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsEqualTo(object other)
        {
            Root.AddFilter(new AttributeFilter.IsEqualTo(Name, other));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsGreaterOrEqualTo(object other)
        {
            Root.AddFilter(new AttributeFilter.IsGreaterOrEqualTo(Name, other));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsGreaterThan(object other)
        {
            Root.AddFilter(new AttributeFilter.IsGreaterThan(Name, other));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsIn(IEnumerable<object> values)
        {
            Root.AddFilter(new AttributeFilter.IsIn(Name, values));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsLessOrEqualTo(object other)
        {
            Root.AddFilter(new AttributeFilter.IsLessOrEqualTo(Name, other));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsLessThan(object other)
        {
            Root.AddFilter(new AttributeFilter.IsLessThan(Name, other));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsMaximal()
        {
            throw new System.NotImplementedException();
        }

        public IFilterSequence<T> IsMinimal()
        {
            throw new System.NotImplementedException();
        }

        public IFilterSequence<T> IsNotBetween(object from, object to)
        {
            Root.AddFilter(new AttributeFilter.IsNotBetween(Name, from, to));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsNotEqualTo(object other)
        {
            Root.AddFilter(new AttributeFilter.IsNotEqualTo(Name, other));
            return new MemoryFilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsNotIn(IEnumerable<object> values)
        {
            Root.AddFilter(new AttributeFilter.IsNotIn(Name, values));
            return new MemoryFilterSequence<T>(Root);
        }
    }
}