namespace Graphene.Query.SubGraph
{
    public interface IResolvable
    {
        IReadOnlyGraph Resolve();
    }
}