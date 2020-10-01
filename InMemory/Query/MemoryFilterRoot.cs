using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryFilterRoot<T> : IFilterRoot<T>
    {
        public MemoryFilterRoot(T reference)
        {
            Reference = reference;
            Filters = new List<AttributeFilter>();
            Sequences = new List<FilterSequenceMode>();
        }

        private List<AttributeFilter> Filters { get; }

        private T Reference { get; }

        private List<FilterSequenceMode> Sequences { get; }

        internal void AddFilter(AttributeFilter filter)
        {
            Filters.Add(filter);
        }

        internal void AddSequence(FilterSequenceMode sequenceMode)
        {
            Sequences.Add(sequenceMode);
        }

        public IFilterAttribute<T> Attribute(string name)
        {
            return new MemoryFilterAttribute<T>(this, name); 
        }

        public T EndWhere()
        {
            return Reference;
        }

        public IFilterLabel<T> Label()
        {
            throw new System.NotImplementedException();
        }
    }
}