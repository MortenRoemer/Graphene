using System;
using System.Collections.Generic;

namespace Graphene.Query.SubGraph
{
    public interface IVertices : IResolvable
    {
        IVertices Where(Func<IReadOnlyVertex, bool> filter);

        IVerticesWithEdges WithEdges();
        
        IVerticesWithEdges WithEdges(IEnumerable<int>? ids);
    }
}