using System.Collections.Generic;

namespace Graphene.Query.SubGraph
{
    public interface IRoot
    {
        IVertices Vertices();
        
        IVertices Vertices(IEnumerable<int> ids);

        IEdges Edges();

        IEdges Edges(IEnumerable<int> ids);
    }
}