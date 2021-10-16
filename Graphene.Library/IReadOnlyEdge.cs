namespace Graphene
{
    public interface IReadOnlyEdge : IReadOnlyEntity
    {
        IReadOnlyVertex FromVertex { get; }

        IReadOnlyVertex ToVertex { get; }

        bool Directed { get; }
    }
}