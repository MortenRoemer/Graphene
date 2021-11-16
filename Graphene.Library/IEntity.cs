using System;

namespace Graphene
{
    public interface IEntity : IReadOnlyEntity
    {
        new IAttributeSet Attributes { get; }
    }

    public static class EntityExtension
    {
        public static void Set(this IEntity entity, string name, object? value)
        {
            entity.Attributes.Set(name, value);
        }
        
        public static Vertex Patch(this IReadOnlyVertex sourceVertex)
        {
            return new Vertex(sourceVertex.Label, sourceVertex.Id);
        }

        public static Edge Patch(this IReadOnlyEdge sourceEdge)
        {
            return new Edge(
                sourceEdge.Label, 
                sourceEdge.FromVertex, 
                sourceEdge.ToVertex, 
                sourceEdge.Directed,
                sourceEdge.Id);
        }
    }
}