using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class FilterRoot<T> : IFilterRoot<T>
    {
        internal FilterRoot(T reference)
        {
            Reference = reference;
            Filters = new List<Filter>();
            Sequences = new List<FilterSequenceMode>();
        }

        private List<Filter> Filters { get; }

        private T Reference { get; }

        private List<FilterSequenceMode> Sequences { get; }

        internal void AddFilter(Filter filter)
        {
            Filters.Add(filter);
        }

        internal void AddSequence(FilterSequenceMode sequenceMode)
        {
            Sequences.Add(sequenceMode);
        }

        public IFilterAttribute<T> Attribute(string name)
        {
            return new FilterAttribute<T>(this, name); 
        }

        public T EndWhere()
        {
            return Reference;
        }

        public IFilterLabel<T> Label()
        {
            return new FilterLabel<T>(this);
        }
    }
}