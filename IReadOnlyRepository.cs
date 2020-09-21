using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphene {
    public interface IReadOnlyRepository<T> : IEnumerable<T>
    {
        long Count();

        IEnumerable<T> Get(IEnumerable<Guid> ids);

        bool Contains(IEnumerable<Guid> ids);
    }

    public static class ReadOnlyRepositoryExtension {
        public static T Get<T>(this IReadOnlyRepository<T> repository, Guid id) {
            return repository.Get(new[] {id}).FirstOrDefault();
        }

        public static bool Contains<T>(this IReadOnlyRepository<T> repository, Guid id) {
            return repository.Contains(new[] {id});
        }
    }
}