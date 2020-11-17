using System;

namespace Graphene.Query.SubGraph
{
    public interface IEdges : IResolvable
    {
        IEdges Where(Func<IReadOnlyEdge, bool> filter);
    }
}