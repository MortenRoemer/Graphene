namespace Graphene
{
    public interface IEdge : IEntity, IReadOnlyEdge
    {
        new IReadOnlyRepository<IVertex> Vertices { get; }

        new IVertex FromVertex { get; }

        new IVertex ToVertex { get; }
    }
}