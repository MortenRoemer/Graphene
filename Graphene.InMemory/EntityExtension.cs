namespace Graphene.InMemory
{
    public static class EntityExtension
    {
        public static MemoryVertex Patch(this IReadOnlyVertex sourceVertex)
        {
            return new MemoryVertex(sourceVertex.Label, sourceVertex.Id);
        }

        public static MemoryEdge Patch(this IReadOnlyEdge sourceEdge)
        {
            return new MemoryEdge(
                sourceEdge.Label, 
                sourceEdge.FromVertex, 
                sourceEdge.ToVertex, 
                sourceEdge.Directed,
                sourceEdge.Id);
        }
    }
}