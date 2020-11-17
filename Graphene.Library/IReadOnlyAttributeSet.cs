using System.Collections.Generic;

namespace Graphene
{
    public interface IReadOnlyAttributeSet : IEnumerable<KeyValuePair<string, object>>
    {
        object this[string name] { get; }
        
        int Count { get; }

        IEnumerable<string> Names { get; }

        IEnumerable<object> Values { get; }

        bool TryGet<T>(string name, out T value);

        T GetOrDefault<T>(string name, T defaultValue);
    }
}