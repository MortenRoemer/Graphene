using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryFilterRoot<T> : IFilterRoot<T>
    {
        public MemoryFilterRoot(T reference)
        {
            Reference = reference;
        }

        private T Reference { get; }

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
    }
}