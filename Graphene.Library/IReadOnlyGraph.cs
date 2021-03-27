using Graphene.Query;

namespace Graphene
{
    public interface IReadOnlyGraph
    {
        int Size { get; }
        
        int DataVersion { get; }

        IReadOnlyRepository<IReadOnlyVertex> Vertices { get; }

        IReadOnlyRepository<IReadOnlyEdge> Edges { get; }

        IGraph Clone();

        IQueryRoot Select();
    }
}