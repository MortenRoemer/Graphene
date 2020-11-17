namespace Graphene
{
    public interface IVertex : IEntity, IReadOnlyVertex
    {
        new IReadOnlyRepository<IEdge> Edges { get; }
        
        new IVertexEdgeRepository IngoingEdges { get; }

        new IVertexEdgeRepository OutgoingEdges { get; }

        new IVertexEdgeRepository BidirectionalEdges { get; }
    }
}