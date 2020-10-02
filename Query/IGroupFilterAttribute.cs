using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IGroupFilterAttribute<T>
    {
        IGroupFilterSequence<T> HasValue();

        IGroupFilterSequence<T> HasNoValue();

        IGroupFilterSequence<T> IsBetween<V>(V from, V to);

        IGroupFilterSequence<T> IsEqualTo<V>(V other);

        IGroupFilterSequence<T> IsGreaterThan<V>(V other);

        IGroupFilterSequence<T> IsGreaterOrEqualTo<V>(V other);

        IGroupFilterSequence<T> IsIn<V>(IEnumerable<V> values);

        IGroupFilterSequence<T> IsLessThan<V>(V other);

        IGroupFilterSequence<T> IsLessOrEqualTo<V>(V other);

        IGroupFilterSequence<T> IsNotBetween<V>(V from, V to);

        IGroupFilterSequence<T> IsNotEqualTo<V>(V other);

        IGroupFilterSequence<T> IsNotIn<V>(IEnumerable<V> values);
    }
}