using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.Query.Filter
{
    public class AndGroupFilter : EntityFilter
    {
        public AndGroupFilter(IEnumerable<EntityFilter> children)
        {
            Children = children ?? throw new ArgumentNullException(nameof(children));
        }

        public IEnumerable<EntityFilter> Children { get; }

        public override bool Contains(IEntity entity)
        {
            return Children.All(child => child.Contains(entity));
        }
    }
}