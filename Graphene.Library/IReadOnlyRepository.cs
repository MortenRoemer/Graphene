using System.Collections.Generic;
using System.Linq;

namespace Graphene
{
    public interface IReadOnlyRepository<out T> : IEnumerable<T> where T : IReadOnlyEntity
    {
        int Count();

        IEnumerable<T> Get(IEnumerable<int> ids);

        T Get(int id);

        bool Contains(IEnumerable<int> ids);

        bool Contains(int id);
    }

    public static class ReadOnlyRepositoryExtension {
        public static bool Contains<T>(this IReadOnlyRepository<T> repository, T item) where T : IReadOnlyEntity
        {
            return repository.Contains(item.Id);
        }
        
        public static bool Contains<T>(this IReadOnlyRepository<T> repository, IEnumerable<T> items) where T : IReadOnlyEntity
        {
            return repository.Contains(items.Select(item => item.Id));
        }
    }
}