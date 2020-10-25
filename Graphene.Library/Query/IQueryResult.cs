using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryResult : IEnumerable<IEntity>
    {
        IGraph Graph { get; }

        bool FindNextResult(out IQueryResult next);
    }
}