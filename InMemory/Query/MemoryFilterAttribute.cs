using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryFilterAttribute<T> : IFilterAttribute<T>
    {
        public MemoryFilterAttribute(MemoryFilterRoot<T> root, string name)
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
            throw new System.NotImplementedException();
        }

        public IFilterSequence<T> IsEqualTo(object other)
        {
            throw new System.NotImplementedException();
        }

        public IFilterSequence<T> IsGreaterOrEqualTo(object other)
        {
            throw new System.NotImplementedException();
        }

        public IFilterSequence<T> IsGreaterThan(object other)
        {
            throw new System.NotImplementedException();
        }

        public IFilterSequence<T> IsIn(IEnumerable<object> values)
        {
            throw new System.NotImplementedException();
        }

        public IFilterSequence<T> IsLessOrEqualTo(object other)
        {
            throw new System.NotImplementedException();
        }

        public IFilterSequence<T> IsLessThan(object other)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public IFilterSequence<T> IsNotEqualTo(object other)
        {
            throw new System.NotImplementedException();
        }

        public IFilterSequence<T> IsNotIn(IEnumerable<object> values)
        {
            throw new System.NotImplementedException();
        }
    }
}