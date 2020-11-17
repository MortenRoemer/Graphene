using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory
{
    public class MemoryAttributeSet : IAttributeSet
    {
        internal MemoryAttributeSet()
        {
            Attributes = new Lazy<IDictionary<string, object>>(() => new SortedDictionary<string, object>(), isThreadSafe: false);
        }

        private Lazy<IDictionary<string, object>> Attributes { get; }

        public object this[string name]
        {
            get => TryGet(name, out object result) ? result : throw new KeyNotFoundException($"attribute with name {name} is not present");
            set => Set(name, value);
        }

        public int Count => Attributes.IsValueCreated ? Attributes.Value.Count : 0;

        public IEnumerable<string> Names => Attributes.IsValueCreated ? Attributes.Value.Keys : Enumerable.Empty<string>();

        public IEnumerable<object> Values => Attributes.IsValueCreated ? Attributes.Value.Values : Enumerable.Empty<object>();

        public void Clear()
        {
            if (Attributes.IsValueCreated)
                Attributes.Value.Clear();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Attributes.IsValueCreated ? Attributes.Value.GetEnumerator() : Enumerable.Empty<KeyValuePair<string, object>>().GetEnumerator();
        }

        public void Set(string name, object value)
        {
            Attributes.Value[name] = value;
        }

        public bool TryGet<T>(string name, out T value)
        {
            if (!Attributes.IsValueCreated)
            {
                value = default;
                return false;
            }

            var found = Attributes.Value.TryGetValue(name, out var result);
            value = (T)result;
            return found;
        }

        public T GetOrDefault<T>(string name, T defaultValue)
        {
            try
            {
                return TryGet(name, out T result) ? result : defaultValue;
            }
            catch (InvalidCastException)
            {
                return defaultValue;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}