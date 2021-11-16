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
            Id == other.Id;

        public override int GetHashCode() =>
            Id.GetHashCode();

        public static bool operator ==(EntityReference left, EntityReference right)
        {
            return left.Id == right.Id;
        }

        public static bool operator !=(EntityReference left, EntityReference right)
        {
            return left.Id != right.Id;
        }
    }
}