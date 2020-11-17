using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class FromVertex : IFromVertex
    {
        internal FromVertex(Root root, int vertexId)
        {
            Root = root;
            VertexId = vertexId;
        }
        
        internal Root Root { get; }
        
        internal int VertexId { get; }
        
        public IWithMinimalEdges WithMinimalEdges()
        {
            return new WithMinimalEdges(this);
        }
    }
}