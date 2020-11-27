using System.Collections.Generic;
using System.Linq;

namespace Graphene
{
    public interface IRepository<out T> : IReadOnlyRepository<T> where T : IEntity
    {
        void Delete(IEnumerable<int> ids);

        void Delete(int id);

        void Clear();
    }

    public static class RepositoryExtension {
        public static void Delete<T>(this IRepository<T> repository, IEnumerable<T> items) where T : IEntity
        {
            repository.Delete(items.Select(item => item.Id));
        }
        
        public static void Delete<T>(this IRepository<T> repository, T item) where T : IEntity
        {
            repository.Delete(item.Id);
        }
    }
}