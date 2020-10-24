using System;

namespace Graphene
{
    public interface IEdge : IEntity
    {
        IReadOnlyRepository<IVertex> Vertices { get; }

        IVertex FromVertex { get; }

        IVertex ToVertex { get; }

        bool Directed { get; }
    }
}