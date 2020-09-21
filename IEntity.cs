using System;

namespace Graphene
{
    public interface IEntity
    {
        IGraph Graph { get; }

        Guid Id { get; }

        string Label { get; set; }

        IAttributeSet Attributes { get; }
    }
}