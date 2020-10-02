using System;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryQueryBuilderOptimizerAggregate : IOptimizerAggregate<MemoryQueryBuilderRoute>
    {
        internal MemoryQueryBuilderOptimizerAggregate(MemoryQueryBuilderRoute reference, OptimizerAggregateMode aggregateMode)
        {
            Reference = reference ?? throw new ArgumentNullException(nameof(reference));
            AggregateMode = aggregateMode;
        }

        private OptimizerAggregateMode AggregateMode { get; }

        private MemoryQueryBuilderRoute Reference { get; }

        public IOptimizerAttribute<MemoryQueryBuilderRoute> Attribute(string name)
        {
            return new MemoryQueryBuilderOptimizerAttribute(Reference, AggregateMode, name);
        }
    }
}