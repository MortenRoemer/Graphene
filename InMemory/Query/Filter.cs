namespace Graphene.InMemory.Query
{
    internal abstract class Filter
    {
        public abstract bool Contains(IEntity entity);
    }
}