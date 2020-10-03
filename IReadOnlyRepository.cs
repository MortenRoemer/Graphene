using System.Collections.Generic;

namespace Graphene
{
    public interface IReadOnlyRepository<T> : IEnumerable<T>
    {
        long Count();

        IEnumerable<T> Get(IEnumerable<long> ids);

        bool Contains(IEnumerable<long> ids);
    }

    public static class ReadOnlyRepositoryExtension {
        public static T Get<T>(this IReadOnlyRepository<T> repository, long id) {
            using (var enumerator = repository.Get(new[] {id}).GetEnumerator())
            {
                return enumerator.MoveNext() ? enumerator.Current : default;
            }
        }

        public static bool Contains<T>(this IReadOnlyRepository<T> repository, long id) {
            return repository.Contains(new[] {id});
        }
    }
}