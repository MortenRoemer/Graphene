using System.Collections.Generic;

namespace Graphene.Query.Route
{
    public interface IRouteResult
    {
        bool Found { get; }
        
        IReadOnlyVertex Origin { get; }
        
        IReadOnlyList<RouteStep> Steps { get; }
    }
}