using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IFilterAttribute<T>
    {
        IFilterSequence<T> HasValue();

        IFilterSequence<T> HasNoValue();

        IFilterSequence<T> IsBetween(object from, object to);

        IFilterSequence<T> IsEqualTo(object other);

        IFilterSequence<T> IsGreaterThan(object other);

        IFilterSequence<T> IsGreaterOrEqualTo(object other);

        IFilterSequence<T> IsIn(IEnumerable<object> values);

        IFilterSequence<T> IsLessThan(object other);

        IFilterSequence<T> IsLessOrEqualTo(object other);

        IFilterSequence<T> IsMaximal();

        IFilterSequence<T> IsMinimal();

        IFilterSequence<T> IsNotBetween(object from, object to);

        IFilterSequence<T> IsNotEqualTo(object other);

        IFilterSequence<T> IsNotIn(IEnumerable<object> values);
    }
}