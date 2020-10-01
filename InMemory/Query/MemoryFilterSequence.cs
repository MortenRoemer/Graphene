using System;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryFilterSequence<T> : IFilterSequence<T>
    {
        internal MemoryFilterSequence(MemoryFilterRoot<T> root)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
        }

        private MemoryFilterRoot<T> Root { get; }

        public IFilterNode<T> And()
        {
            Root.AddSequence(FilterSequenceMode.And);
            return Root;
        }

        public IFilterAttribute<T> Attribute(string name)
        {
            Root.AddSequence(FilterSequenceMode.And);
            return new MemoryFilterAttribute<T>(Root, name);
        }

        public T EndWhere()
        {
            return Root.EndWhere();
        }

        public IFilterLabel<T> Label()
        {
            Root.AddSequence(FilterSequenceMode.And);
            return new MemoryFilterLabel<T>(Root);
        }

        public IFilterNode<T> Or()
        {
            Root.AddSequence(FilterSequenceMode.Or);
            return Root;
        }

        public IFilterNode<T> Xor()
        {
            Root.AddSequence(FilterSequenceMode.Xor);
            return Root;
        }
    }
}