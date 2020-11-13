using Graphene.Query;

namespace Graphene
{
    public interface IGraph
    {
        int Size { get; }

        IVertexRepository Vertices { get; }

        IReadOnlyRepository<IEdge> Edges { get; }

        void Clear();

        IGraph Clone();

        void Merge(IGraph other);

        IQueryBuilderRoot Select();
    }
}
