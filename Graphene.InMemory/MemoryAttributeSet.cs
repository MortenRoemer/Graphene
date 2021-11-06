using System;
using System.Collections;
using System.Collections.Generic;

namespace Graphene.InMemory
{
    public class MemoryAttributeSet : IAttributeSet
    {
        internal MemoryAttributeSet() { }
        
        private Lazy<IDictionary<string, object?>> Attributes { get; } = new(() => new SortedDictionary<string, object?>(), isThreadSafe: false);
        
        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        {
            return Attributes.IsValueCreated
                ? Attributes.Value.GetEnumerator()
                : new List<KeyValuePair<string, object?>>(0).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T? Get<T>(string name)
        {
            return Attributes.IsValueCreated && Attributes.Value.TryGetValue(name, out var value)
                ? (T?) value
                : throw new ArgumentException($"attributes does not contain value for key: {name}");
        }

        public int Count => Attributes.IsValueCreated 
            ? Attributes.Value.Count 
            : 0;
        
        public IEnumerable<string> Names => Attributes.IsValueCreated 
            ? Attributes.Value.Keys 
            : Array.Empty<string>();

        public IEnumerable<object?> Values => Attributes.IsValueCreated
            ? Attributes.Value.Values
            : Array.Empty<object?>();
        
        public bool TryGet<T>(string name, out T? value)
        {
            if (Attributes.IsValueCreated && Attributes.Value.TryGetValue(name, out var unCastValue))
            {
                value = (T?) unCastValue;
                return true;
            }

            value = default;
            return false;
        }

        public T? GetOrDefault<T>(string name, T? defaultValue)
        {
            return Attributes.IsValueCreated && Attributes.Value.TryGetValue(name, out var unCastValue)
                ? (T?) unCastValue
                : defaultValue;
        }

        public void Set(string name, object? value)
        {
            Attributes.Value[name] = value;
        }

        public void Clear()
        {
            if (Attributes.IsValueCreated)
                Attributes.Value.Clear();
        }
    }
}