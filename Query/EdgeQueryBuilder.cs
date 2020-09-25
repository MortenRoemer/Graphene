using System;
using System.Collections.Generic;
using Graphene.Query.Filter;

namespace Graphene.Query
{
    public class EdgeQueryBuilder : EntityQueryBuilder
    {
        public EdgeQueryBuilder(QueryBuilder parent) : base(parent)
        {
        }

        public EdgeQueryBuilder(QueryBuilder parent, IEnumerable<Guid> range) : base(parent)
        {
            Range = range;
        }

        public IEnumerable<Guid> Range { get; set; }

        public FilterBuilder Where()
        {
            throw new NotImplementedException();
        }
    }
}