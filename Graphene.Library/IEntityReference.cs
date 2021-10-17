using System;

namespace Graphene
{
    public interface IEntityReference
    {
        Guid Id { get; }

        string Label { get; }
        
        EntityClass EntityClass { get; }
    }
}