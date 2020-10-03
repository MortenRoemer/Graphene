using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQuerySolution : IEnumerable<IEntity>
    {
        long Length { get; }
    }
}