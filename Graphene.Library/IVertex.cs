namespace Graphene
{
    public interface IVertex : IEntity
    {
        IVertexEdgeRepository IngoingEdges { get; }

        IVertexEdgeRepository OutgoingEdges { get; }

        IVertexEdgeRepository BidirectionalEdges { get; }
    }
}