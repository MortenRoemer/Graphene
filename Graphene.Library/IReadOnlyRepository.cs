using System.Collections.Generic;

namespace Graphene
{
    public interface IReadOnlyRepository<T> : IEnumerable<T>
    {
        ulong Count();

        IEnumerable<T> Get(IEnumerable<ulong> ids);

        bool Contains(IEnumerable<ulong> ids);
    }

    public static class ReadOnlyRepositoryExtension {
        public static T Get<T>(this IReadOnlyRepository<T> repository, ulong id) {
            using var enumerator = repository.Get(new[] { id }).GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : default;
        }

        public static bool Contains<T>(this IReadOnlyRepository<T> repository, ulong id) {
            return repository.Contains(new[] {id});
        }
    }
}