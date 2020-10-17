using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class AgentResult : IQueryResult
    {
        internal AgentResult(BuilderRoot query, IEnumerable<IEntity> entities)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
            Entities = entities?.ToArray() ?? throw new ArgumentNullException(nameof(entities));
        }

        public IGraph Graph => Query.Graph;

        private BuilderRoot Query { get; }

        private IEntity[] Entities { get; } 

        public bool FindNextResult(out IQueryResult next)
        {
            var agent = new QueryAgent(Query, Entities.Select(entities => entities.Id).ToArray());
            return agent.FindSolution(out next);
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            return Entities.Cast<IEntity>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}