using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory
{
    public class MemoryAttributeSet : IAttributeSet
    {
        internal MemoryAttributeSet(MemoryGraph graph)
        {
            Graph = graph;
            Attributes = new Lazy<IDictionary<string, object>>(() => new SortedDictionary<string, object>(), isThreadSafe: false);
        }

        private MemoryGraph Graph { get; }
        
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
            if (!Attributes.IsValueCreated)
                return;
            
            Attributes.Value.Clear();
            Graph.DataVersion++;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Attributes.IsValueCreated ? Attributes.Value.GetEnumerator() : Enumerable.Empty<KeyValuePair<string, object>>().GetEnumerator();
        }

        public void Set(string name, object value)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            
            Attributes.Value[name] = value;
            Graph.DataVersion++;
        }

        public bool TryGet<T>(string name, out T value)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            
            if (!Attributes.IsValueCreated)
            {
                value = default;
                return false;
            }

            var found = Attributes.Value.TryGetValue(name, out var result);

            if (!found)
            {
                value = default;
                return false;
            }
            
            try
            {
                value = (T)result;
                return true;
            }
            catch (InvalidCastException)
            {
                value = default;
                return false;
            }
        }

        public T GetOrDefault<T>(string name, T defaultValue)
        {
            return TryGet(name, out T result) ? result : defaultValue;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}