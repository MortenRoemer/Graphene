using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryRoot
    {
        Entity.IVertices Vertices();
        
        Entity.IVertices Vertices(IEnumerable<int> range);

        Entity.IEdges Edges();
        
        Entity.IEdges Edges(IEnumerable<int>? range);
        
        Route.IRoot Route();
        
        SubGraph.IRoot SubGraph();
    }
}