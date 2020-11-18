using Graphene.Query;

namespace Graphene
{
    public interface IGraph : IReadOnlyGraph
    {
        new IVertexRepository Vertices { get; }
        
        new IReadOnlyRepository<IReadOnlyEdge> Edges { get; }

        void Clear();

        void Merge(IReadOnlyGraph other);
    }
}
