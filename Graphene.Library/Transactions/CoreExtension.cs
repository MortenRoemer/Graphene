namespace Graphene.Transactions
{
    public static class CoreExtension
    {
        public static CreateVertex ToCreateVertexAction(this IReadOnlyVertex vertex)
            => new CreateVertex(vertex);

        public static CreateEdge ToCreateEdgeAction(this IReadOnlyEdge edge)
            => new CreateEdge(edge);

        public static UpdateVertex ToUpdateVertexAction(this IReadOnlyVertex vertex)
            => new UpdateVertex(vertex);

        public static UpdateEdge ToUpdateEdgeAction(this IReadOnlyEdge edge)
            => new UpdateEdge(edge);
        
        public static DeleteEntity ToDeleteEntityAction(this IEntityReference entityReference)
            => new DeleteEntity(entityReference);
    }
}