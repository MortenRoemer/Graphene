using System;

namespace Graphene.Query
{
    public abstract class EntityQueryBuilder
    {
        protected EntityQueryBuilder(QueryBuilder parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        private QueryBuilder Parent { get; }
    }
}