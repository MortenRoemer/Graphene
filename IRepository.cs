using System.Collections.Generic;

namespace Graphene
{
    public interface IRepository<T> : IReadOnlyRepository<T>
    {
        void Update(IEnumerable<T> items);

        void Delete(IEnumerable<T> items);

        void Clear();
    }

    public static class RepositoryExtension {
        public static void Update<T>(this IRepository<T> repository, T item) {
            repository.Update(new[] {item});
        }

        public static void Delete<T>(this IRepository<T> repository, T item) {
            repository.Delete(new[] {item});
        }
    }
}