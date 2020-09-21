using System;

namespace Graphene
{
    public interface IEdge : IEntity
    {
        IVertex FromVertex { get; }

        IVertex ToVertex { get; }

        bool Directed { get; }
    }
}