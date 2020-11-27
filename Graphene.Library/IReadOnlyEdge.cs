namespace Graphene
{
    public interface IReadOnlyEdge : IReadOnlyEntity, IPromotable<IEdge>
    {
        IReadOnlyRepository<IReadOnlyVertex> Vertices { get; }

        IReadOnlyVertex FromVertex { get; }

        IReadOnlyVertex ToVertex { get; }

        bool Directed { get; }
    }
}