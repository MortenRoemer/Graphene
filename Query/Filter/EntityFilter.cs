namespace Graphene.Query.Filter
{
    public abstract class EntityFilter
    {
        public static FilterBuilder Build()
        {
            return new FilterBuilder();
        }

        public abstract bool Contains(IEntity entity);
    }
}