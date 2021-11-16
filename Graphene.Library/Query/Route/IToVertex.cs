using System.Threading.Tasks;

namespace Graphene.Query.Route
{
    public interface IToVertex<TMetric>
    {
        Task<RouteResult<TMetric>> Resolve();
    }
}