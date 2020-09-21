namespace Graphene.Query.Filter
{
    public abstract class EntityFilter
    {
        public abstract bool Contains(IEntity entity);
    }
}