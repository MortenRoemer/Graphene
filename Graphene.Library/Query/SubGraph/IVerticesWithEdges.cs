using System;

namespace Graphene.Query.SubGraph
{
    public interface IVerticesWithEdges : IResolvable
    {
        IVerticesWithEdges Where(Func<IReadOnlyEdge, bool> filter);
    }
}