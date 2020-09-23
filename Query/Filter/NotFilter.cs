using System;

namespace Graphene.Query.Filter
{
    public class NotFilter : EntityFilter
    {
        public NotFilter(EntityFilter child)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        public EntityFilter Child { get; }

        public override bool Contains(IEntity entity) => !Child.Contains(entity);
    }
}