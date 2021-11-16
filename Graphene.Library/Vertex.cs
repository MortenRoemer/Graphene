using System;

namespace Graphene
{
    public class Vertex : IVertex
    {
        public Vertex(string label)
        {
            Id = Guid.NewGuid();
            Label = label;
        }
        
        public Vertex(string label, Guid id)
        {
            Id = id;
            Label = label;
        }
        
        public Guid Id { get; }
        
        public string Label { get; }

        public IAttributeSet Attributes { get; } = new AttributeSet();

        public Vertex WithAttribute(string name, object? value)
        {
            Attributes.Set(name, value);
            return this;
        }
        
        public EntityClass EntityClass => EntityClass.Vertex;
        
        IReadOnlyAttributeSet IReadOnlyEntity.Attributes => Attributes;

        public override bool Equals(object? obj)
        {
            return obj is IReadOnlyVertex other &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EntityClass, Id);
        }
    }
}