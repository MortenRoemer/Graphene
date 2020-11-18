namespace Graphene.Query
{
    public interface IQueryRoot
    {
        Route.IRoot Route();
        
        SubGraph.IRoot SubGraph();
    }
}