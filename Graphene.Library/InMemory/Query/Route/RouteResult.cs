using System.Collections.Generic;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class RouteResult : IRouteResult
    {
        internal RouteResult(bool found, IReadOnlyVertex origin, IReadOnlyList<RouteStep> steps)
        {
            Found = found;
            Origin = origin;
            Steps = steps;
        }
        
        public bool Found { get; }
        
        public IReadOnlyVertex Origin { get; }
        
        public IReadOnlyList<RouteStep> Steps { get; }
    }
}