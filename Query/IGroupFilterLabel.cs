using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IGroupFilterLabel<T>
    {
        IGroupFilterSequence<T> DoesMatch(string pattern);

        IGroupFilterSequence<T> DoesNotMatch(string pattern);

        IGroupFilterSequence<T> IsEqualTo(string other);

        IGroupFilterSequence<T> IsNotEqualTo(string other);

        IGroupFilterSequence<T> IsIn(IEnumerable<string> range);

        IGroupFilterSequence<T> IsNotIn(IEnumerable<string> range);
    }
}