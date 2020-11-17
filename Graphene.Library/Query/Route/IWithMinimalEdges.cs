using System;

namespace Graphene.Query.Route
{
    public interface IWithMinimalEdges
    {
        IToVertex ToVertex(int vertexId);
        
        IWithMinimalEdges Where(Func<IReadOnlyEdge, bool> filter);
    }
}