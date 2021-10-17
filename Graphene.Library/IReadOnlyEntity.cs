using System;

namespace Graphene
{
    public interface IReadOnlyEntity : IEntityReference
    {
        IReadOnlyAttributeSet Attributes { get; }
    }

    public static class ReadOnlyEntityExtension
    {
        public static T? Get<T>(this IReadOnlyEntity entity, string name)
            => entity.Attributes.Get<T>(name);
    }
}