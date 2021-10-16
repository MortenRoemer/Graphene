using System.Collections.Generic;

namespace Graphene
{
    public interface IVertex : IEntity, IReadOnlyVertex
    {
        new ICollection<IEdge> Edges { get; }
    }
}