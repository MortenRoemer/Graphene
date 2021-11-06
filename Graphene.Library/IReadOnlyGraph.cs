using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Graphene.Query;

namespace Graphene
{
    public interface IReadOnlyGraph
    {
        string Name { get; }

        IQueryRoot Select();

        Task<IReadOnlyEntity> Get(Guid id);
        
        Task<IFindResult<IReadOnlyEntity>> FindEntities(
            int pageSize,
            Expression<Func<IReadOnlyEntity, bool>>? filter = null);
        
        Task<IFindResult<IReadOnlyVertex>> FindVertices(
            int pageSize,
            Expression<Func<IReadOnlyVertex, bool>>? filter = null);
        
        Task<IFindResult<IReadOnlyEdge>> FindEdges(
            int pageSize,
            Expression<Func<IReadOnlyEdge, bool>>? filter = null);
    }
}