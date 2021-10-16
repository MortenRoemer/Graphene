using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory.Query
{
    public class MemoryVertexView : IReadOnlyRepository<IReadOnlyVertex>
    {
        internal MemoryVertexView(IReadOnlyRepository<IReadOnlyVertex> backend, IEnumerable<int> range)
        {
            Backend = backend;
            Range = range is ISet<int> set ? set : new HashSet<int>(range);
        }
        
        private IReadOnlyRepository<IReadOnlyVertex> Backend { get; }
        
        private ISet<int> Range { get; }
        
        public IEnumerator<IReadOnlyVertex> GetEnumerator()
        {
            return Backend.Where(vertex => Range.Contains(vertex.Id)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count()
        {
            return Range.Count;
        }

        public IEnumerable<IReadOnlyVertex> Get(IEnumerable<int>? ids)
        {
            if (ids is null)
                throw new ArgumentNullException(nameof(ids));
            
            return Backend.Get(ids.Where(id => Range.Contains(id)));
        }

        public IReadOnlyVertex? Get(int id)
        {
            return Range.Contains(id) ? Backend.Get(id) : throw new ArgumentException($"id {id} does not exist");
        }

        public bool Contains(IEnumerable<int>? ids)
        {
            return ids?.All(id => Range.Contains(id)) ?? true;
        }

        public bool Contains(int id)
        {
            return Range.Contains(id);
        }
    }
}