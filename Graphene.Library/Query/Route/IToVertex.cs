namespace Graphene.Query.Route
{
    public interface IToVertex<TMetric>
    {
        RouteResult<TMetric> Resolve();
    }
}