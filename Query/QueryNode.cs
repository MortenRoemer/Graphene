namespace Graphene.Query
{
    public abstract class QueryNode
    {
        public int? Limit { get; set; }

        public bool? IncludeNode { get; set; }

        public abstract bool Contains(IEntity entity);
    }
}