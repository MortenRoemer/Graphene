using System;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryFilterSequence<T> : IFilterSequence<T>
    {
        public MemoryFilterSequence(MemoryFilterRoot<T> root)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
        }

        private MemoryFilterRoot<T> Root { get; }

        public IFilterNode<T> And()
        {
            throw new System.NotImplementedException();
        }

        public IFilterAttribute<T> Attribute(string name)
        {
            throw new System.NotImplementedException();
        }

        public T EndWhere()
        {
            throw new System.NotImplementedException();
        }

        public IFilterLabel<T> Label()
        {
            throw new System.NotImplementedException();
        }

        public IFilterNode<T> Or()
        {
            throw new System.NotImplementedException();
        }

        public IFilterNode<T> Xor()
        {
            throw new System.NotImplementedException();
        }
    }
}