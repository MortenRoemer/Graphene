namespace Graphene
{
    public interface IReadOnlyEdge : IReadOnlyEntity
    {
        IReadOnlyRepository<IReadOnlyVertex> Vertices { get; }

        IReadOnlyVertex FromVertex { get; }

        IReadOnlyVertex ToVertex { get; }

        bool Directed { get; }
    }
}