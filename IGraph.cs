namespace Graphene
{
    public interface IGraph
    {
        long Size { get; }

        IRepository<IVertex> Vertices { get; }

        IRepository<IEdge> Edges { get; }

        void Clear();
    }
}
