using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory.Query
{
    public class MemoryEdgeView : IReadOnlyRepository<IReadOnlyEdge>
    {
        internal MemoryEdgeView(IReadOnlyRepository<IReadOnlyEdge> backend, IEnumerable<int> range)
        {
            Backend = backend;
            Range = range is ISet<int> set ? set : new HashSet<int>(range);
        }
        
        private IReadOnlyRepository<IReadOnlyEdge> Backend { get; }
        
        private ISet<int> Range { get; }
        
        public IEnumerator<IReadOnlyEdge> GetEnumerator()
        {
            return Backend.Where(edge => Range.Contains(edge.Id)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count()
        {
            return Range.Count;
        }

        public IEnumerable<IReadOnlyEdge> Get(IEnumerable<int> ids)
        {
            if (ids is null)
                throw new ArgumentNullException(nameof(ids));
            
            return Backend.Get(ids.Where(Range.Contains));
        }

        public IReadOnlyEdge Get(int id)
        {
            return Range.Contains(id) ? Backend.Get(id) : throw new ArgumentException($"id {id} does not exist");
        }

        public bool Contains(IEnumerable<int> ids)
        {
            if (ids is null)
                throw new ArgumentNullException(nameof(ids));
            
            return ids.All(Range.Contains);
        }

        public bool Contains(int id)
        {
            return Range.Contains(id);
        }
    }
}