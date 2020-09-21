using System;
using System.Collections;
using System.Collections.Generic;

namespace Graphene.InMemory
{
    public class MemoryEdgeRepository : IRepository<IEdge>
    {
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public long Count()
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<IEdge> items)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEdge> Get(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IEdge> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<IEdge> items)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}