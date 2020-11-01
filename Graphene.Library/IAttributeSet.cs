using System.Collections.Generic;

namespace Graphene
{
    public interface IAttributeSet : IEnumerable<KeyValuePair<string, object>>
    {
        long Count { get; }

        IEnumerable<string> Names { get; }

        IEnumerable<object> Values { get; }

        bool TryGet<T>(string name, out T value);

        T GetOrDefault<T>(string name, T defaultValue);

        void Set(string name, object value);

        void Clear();
    }
}