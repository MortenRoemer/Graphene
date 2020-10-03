using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    internal class QueryAgent
    {
        public QueryAgent(BuilderRoot query, IEnumerable<long> offset)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
            Offset = offset is null ? null : offset.ToArray();
        }

        private long[] Offset { get; }

        private BuilderRoot Query { get; }

        public bool FindSolution(out IQueryResult result)
        {
            throw new NotImplementedException();
        }
    }
}