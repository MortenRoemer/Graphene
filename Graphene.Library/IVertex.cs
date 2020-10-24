namespace Graphene
{
    public interface IVertex : IEntity
    {
        IReadOnlyRepository<IEdge> Edges { get; }

        IVertexEdgeRepository IngoingEdges { get; }

        IVertexEdgeRepository OutgoingEdges { get; }

        IVertexEdgeRepository BidirectionalEdges { get; }
    }
}