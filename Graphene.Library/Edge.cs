using System;

namespace Graphene
{
    public class Edge : IEdge
    {
        public Edge(string label, Guid fromVertex, Guid toVertex, bool directed)
            : this(label, fromVertex, toVertex, directed, Guid.NewGuid())
        {
        }
        
        public Edge(string label, Guid fromVertex, Guid toVertex, bool directed, Guid id)
        {
            Id = id;
            Label = label;
            FromVertex = fromVertex;
            ToVertex = toVertex;
            Directed = directed;
        }

        public Guid Id { get; }
        
        public string Label { get; }

        public EntityClass EntityClass => EntityClass.Edge;
        
        IReadOnlyAttributeSet IReadOnlyEntity.Attributes => Attributes;

        public IAttributeSet Attributes { get; } = new AttributeSet();
        
        public Edge WithAttribute(string name, object? value)
        {
            Attributes.Set(name, value);
            return this;
        }
        
        public Guid FromVertex { get; }
        
        public Guid ToVertex { get; }
        
        public bool Directed { get; }

        public override bool Equals(object? obj)
        {
            return obj is IReadOnlyEdge other &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EntityClass, Id);
        }
    }
}