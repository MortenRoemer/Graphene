using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.Query.Filter
{
    public class OrGroupFilter : EntityFilter
    {
        public OrGroupFilter(IEnumerable<EntityFilter> children)
        {
            Children = children ?? throw new ArgumentNullException(nameof(children));
        }

        public IEnumerable<EntityFilter> Children { get; }

        public override bool Contains(IEntity entity)
        {
            return Children.Any(child => child.Contains(entity));
        }
    }
}