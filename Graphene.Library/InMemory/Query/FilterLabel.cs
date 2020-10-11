using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class FilterLabel<T> : IFilterLabel<T>
    {
        internal FilterLabel(FilterRoot<T> root)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
        }

        private FilterRoot<T> Root { get; }

        public IFilterSequence<T> DoesMatch(string pattern)
        {
            Root.AddFilter(new LabelFilter.DoesMatch(pattern));
            return new FilterSequence<T>(Root);
        }

        public IFilterSequence<T> DoesNotMatch(string pattern)
        {
            Root.AddFilter(new LabelFilter.DoesNotMatch(pattern));
            return new FilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsEqualTo(string other)
        {
            Root.AddFilter(new LabelFilter.IsEqualTo(other));
            return new FilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsIn(IEnumerable<string> range)
        {
            Root.AddFilter(new LabelFilter.IsIn(range));
            return new FilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsNotEqualTo(string other)
        {
            Root.AddFilter(new LabelFilter.IsNotEqualTo(other));
            return new FilterSequence<T>(Root);
        }

        public IFilterSequence<T> IsNotIn(IEnumerable<string> range)
        {
            Root.AddFilter(new LabelFilter.IsNotIn(range));
            return new FilterSequence<T>(Root);
        }
    }
}