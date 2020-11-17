using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class Root : IRoot
    {
        internal Root(IReadOnlyGraph graph)
        {
            Graph = graph;
        }
        
        internal IReadOnlyGraph Graph { get; }
        
        public IFromVertex FromVertex(int vertexId)
        {
            return new FromVertex(this, vertexId);
        }
    }
}