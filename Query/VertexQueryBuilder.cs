using System;
using System.Collections.Generic;
using Graphene.Query.Filter;

namespace Graphene.Query
{
    public class VertexQueryBuilder : EntityQueryBuilder
    {
        public VertexQueryBuilder(QueryBuilder parent) : base(parent)
        {
        }

        public VertexQueryBuilder(QueryBuilder parent, IEnumerable<Guid> range) : base(parent)
        {
            Range = range;
        }

        private IEnumerable<Guid> Range { get; }

        public FilterBuilder Where()
        {
            throw new NotImplementedException();
        }
    }
}