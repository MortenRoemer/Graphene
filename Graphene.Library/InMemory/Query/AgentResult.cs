using System;
using System.Collections;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class AgentResult : IQueryResult
    {
        internal AgentResult(IGraph graph, IEnumerable<IEntity> entities)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Entities = entities ?? throw new ArgumentNullException(nameof(entities));
        }

        public IGraph Graph { get; }

        private IEnumerable<IEntity> Entities { get; } 

        public bool FindNextResult(out IQueryResult next)
        {
            throw new System.NotImplementedException();
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