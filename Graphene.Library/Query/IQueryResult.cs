using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryResult : IEnumerable<IEntity>
    {
        IGraph ResultGraph { get; }

        bool FindNextResult(out IQueryResult next);
    }
}