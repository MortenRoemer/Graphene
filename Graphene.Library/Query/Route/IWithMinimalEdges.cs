using System;
using System.Linq.Expressions;

namespace Graphene.Query.Route
{
    public interface IWithMinimalEdges
    {
        IToVertex<int> ToVertex(Guid vertexId);
        
        IWithMinimalEdges Where(Expression<Func<IReadOnlyEdge, bool>> filter);
    }
}