using System;

namespace Graphene
{
    public readonly struct EntityReference : IEntityReference
    {
        public EntityReference(Guid id, string label, EntityClass entityClass)
        {
            Id = id;
            Label = label;
            EntityClass = entityClass;
        }
        
        public Guid Id { get; }
        public string Label { get; }
        public EntityClass EntityClass { get; }

        public override bool Equals(object? obj) =>
            obj is IEntityReference other &&
            Id == other.Id &&
            EntityClass == other.EntityClass &&
            Label == other.Label;

        public override int GetHashCode() =>
            Id.GetHashCode() +
            EntityClass.GetHashCode() +
            Label.GetHashCode();

        public static bool operator ==(EntityReference left, EntityReference right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EntityReference left, EntityReference right)
        {
            return !(left == right);
        }
    }
}