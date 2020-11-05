using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class AgentResult : IQueryResult
    {
        internal AgentResult(BuilderRoot query, IEnumerable<IEntity> entities, ulong[] offset)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
            Entities = entities?.ToArray() ?? throw new ArgumentNullException(nameof(entities));
            Offset = offset;
        }

        public IGraph Graph => Query.Graph;

        private BuilderRoot Query { get; }

        private IEnumerable<IEntity> Entities { get; }

        private ulong[] Offset { get; }

        public bool FindNextResult(out IQueryResult next)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}