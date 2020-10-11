using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IFilterAttribute<T>
    {
        IFilterSequence<T> HasValue();

        IFilterSequence<T> HasNoValue();

        IFilterSequence<T> IsBetween<V>(V from, V to);

        IFilterSequence<T> IsEqualTo<V>(V other);

        IFilterSequence<T> IsGreaterThan<V>(V other);

        IFilterSequence<T> IsGreaterOrEqualTo<V>(V other);

        IFilterSequence<T> IsIn<V>(IEnumerable<V> values);

        IFilterSequence<T> IsLessThan<V>(V other);

        IFilterSequence<T> IsLessOrEqualTo<V>(V other);

        IFilterSequence<T> IsNotBetween<V>(V from, V to);

        IFilterSequence<T> IsNotEqualTo<V>(V other);

        IFilterSequence<T> IsNotIn<V>(IEnumerable<V> values);
    }
}