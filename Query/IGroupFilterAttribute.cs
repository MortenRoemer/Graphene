using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IGroupFilterAttribute<T>
    {
        IGroupFilterSequence<T> HasValue();

        IGroupFilterSequence<T> HasNoValue();

        IGroupFilterSequence<T> IsBetween(object from, object to);

        IGroupFilterSequence<T> IsEqualTo(object other);

        IGroupFilterSequence<T> IsGreaterThan(object other);

        IGroupFilterSequence<T> IsGreaterOrEqualTo(object other);

        IGroupFilterSequence<T> IsIn(IEnumerable<object> values);

        IGroupFilterSequence<T> IsLessThan(object other);

        IGroupFilterSequence<T> IsLessOrEqualTo(object other);

        IGroupFilterSequence<T> IsMaximal();

        IGroupFilterSequence<T> IsMinimal();

        IGroupFilterSequence<T> IsNotBetween(object from, object to);

        IGroupFilterSequence<T> IsNotEqualTo(object other);

        IGroupFilterSequence<T> IsNotIn(IEnumerable<object> values);
    }
}