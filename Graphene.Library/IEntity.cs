using System;

namespace Graphene
{
    public interface IEntity : IReadOnlyEntity
    {
        new Guid Id { get; set; }

        new string Label { get; set; }
        
        new IAttributeSet Attributes { get; }
    }

    public static class EntityExtension
    {
        public static void Set(this IEntity entity, string name, object? value)
        {
            entity.Attributes.Set(name, value);
        }
    }
}