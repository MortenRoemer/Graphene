using System;

namespace Graphene
{
    public interface IEdge : IEntity, IReadOnlyEdge
    {
        new Guid FromVertex { get; init; }

        new Guid ToVertex { get; init; }

        new bool Directed { get; init; }
    }
}