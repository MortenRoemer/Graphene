using System.Collections.Generic;

namespace Graphene
{
    public interface IReadOnlyAttributeSet : IEnumerable<KeyValuePair<string, object?>>
    {
        T? Get<T>(string name);
        
        int Count { get; }

        IEnumerable<string> Names { get; }

        IEnumerable<object?> Values { get; }

        bool TryGet<T>(string name, out T? value);

        T? GetOrDefault<T>(string name, T? defaultValue);
    }
}