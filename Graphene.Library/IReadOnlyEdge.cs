using System;

namespace Graphene
{
    public interface IReadOnlyEdge : IReadOnlyEntity
    {
        Guid FromVertex { get; }

        Guid ToVertex { get; }

        bool Directed { get; }
    }
}