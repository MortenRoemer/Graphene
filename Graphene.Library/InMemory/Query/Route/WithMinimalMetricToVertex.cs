using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class WithMinimalMetricToVertex : IToVertex<float>
    {
        internal WithMinimalMetricToVertex(WithMinimalMetric withMinimalMetric, int targetId)
        {
            WithMinimalMetric = withMinimalMetric;
            TargetId = targetId;
        }
        
        private WithMinimalMetric WithMinimalMetric { get; }
        
        private int TargetId { get; }
        
        public RouteResult<float> Resolve()
        {
            var graph = WithMinimalMetric.FromVertex.Root.Graph;
            var origin = graph.Vertices.Get(WithMinimalMetric.FromVertex.VertexId);
            var target = graph.Vertices.Get(TargetId);

            return DjikstraRouteResolver.SolveForMinimalMetric(
                origin,
                target,
                WithMinimalMetric.Filter,
                WithMinimalMetric.MetricFunction,
                ((first, second) => first + second)
            );
        }
    }
}