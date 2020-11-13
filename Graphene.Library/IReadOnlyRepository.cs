using System.Collections.Generic;

namespace Graphene
{
    public interface IReadOnlyRepository<out T> : IEnumerable<T>
    {
        int Count();

        IEnumerable<T> Get(IEnumerable<int> ids);

        bool Contains(IEnumerable<int> ids);
    }

    public static class ReadOnlyRepositoryExtension {
        public static T Get<T>(this IReadOnlyRepository<T> repository, int id) {
            using var enumerator = repository.Get(new[] { id }).GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : default;
        }

        public static bool Contains<T>(this IReadOnlyRepository<T> repository, int id) {
            return repository.Contains(new[] {id});
        }
    }
}