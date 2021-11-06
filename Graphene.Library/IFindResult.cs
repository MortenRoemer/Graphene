using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Graphene
{
    public interface IFindResult<T> where T : IReadOnlyEntity
    {
        int PageSize { get; }
        
        bool MoreResults { get; }
        
        IReadOnlyCollection<T> Results { get; }

        Task<IFindResult<T>> GetMoreResults();
    }
}