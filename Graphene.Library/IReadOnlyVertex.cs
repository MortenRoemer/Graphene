namespace Graphene
{
    public interface IReadOnlyVertex : IReadOnlyEntity
    {
        IReadOnlyRepository<IReadOnlyEdge> Edges { get; }

        IReadOnlyRepository<IReadOnlyEdge> IngoingEdges { get; }

        IReadOnlyRepository<IReadOnlyEdge> OutgoingEdges { get; }

        IReadOnlyRepository<IReadOnlyEdge> BidirectionalEdges { get; }
    }
}