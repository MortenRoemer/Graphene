using System.Collections.Generic;

namespace Graphene
{
    public interface IReadOnlyVertex : IReadOnlyEntity
    {
        IReadOnlyCollection<IReadOnlyEdge> Edges { get; }
    }
}