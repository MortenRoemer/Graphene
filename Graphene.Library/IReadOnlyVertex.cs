namespace Graphene
{
    public interface IReadOnlyVertex : IReadOnlyEntity, IPromotable<IVertex>
    {
        IReadOnlyRepository<IReadOnlyEdge> Edges { get; }

        IReadOnlyRepository<IReadOnlyEdge> IngoingEdges { get; }

        IReadOnlyRepository<IReadOnlyEdge> OutgoingEdges { get; }

        IReadOnlyRepository<IReadOnlyEdge> BidirectionalEdges { get; }
    }
}