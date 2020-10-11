using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IFilterLabel<T>
    {
        IFilterSequence<T> DoesMatch(string pattern);

        IFilterSequence<T> DoesNotMatch(string pattern);

        IFilterSequence<T> IsEqualTo(string other);

        IFilterSequence<T> IsNotEqualTo(string other);

        IFilterSequence<T> IsIn(IEnumerable<string> range);

        IFilterSequence<T> IsNotIn(IEnumerable<string> range);
    }
}