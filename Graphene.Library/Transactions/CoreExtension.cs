namespace Graphene.Transactions
{
    public static class CoreExtension
    {
        public static CreateVertex ToCreateVertexAction(this IReadOnlyVertex vertex)
            => new CreateVertex(vertex);

        public static CreateEdge ToCreateEdgeAction(this IReadOnlyEdge edge)
            => new CreateEdge(edge);

        public static UpdateEntity ToUpdateEntityAction(this IReadOnlyEntity entity)
            => new UpdateEntity(entity);
        
        public static DeleteEntity ToDeleteEntityAction(this IEntityReference entityReference)
            => new DeleteEntity(entityReference);
    }
}