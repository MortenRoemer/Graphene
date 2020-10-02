using System;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryQueryBuilderOptimizer : IQueryBuilderOptimizer<MemoryQueryBuilderRoute>
    {
        internal MemoryQueryBuilderOptimizer(MemoryQueryBuilderRoute reference)
        {
            Reference = reference ?? throw new ArgumentNullException(nameof(reference));
        }

        private MemoryQueryBuilderRoute Reference { get; }

        public IOptimizerAggregate<MemoryQueryBuilderRoute> TheAverageOf()
        {
            return new MemoryQueryBuilderOptimizerAggregate(Reference, OptimizerAggregateMode.Average);
        }

        public IOptimizerAggregate<MemoryQueryBuilderRoute> TheMaximumOf()
        {
            return new MemoryQueryBuilderOptimizerAggregate(Reference, OptimizerAggregateMode.Maximum);
        }

        public IOptimizerAggregate<MemoryQueryBuilderRoute> TheMinimumOf()
        {
            return new MemoryQueryBuilderOptimizerAggregate(Reference, OptimizerAggregateMode.Minimum);
        }

        public IOptimizerAggregate<MemoryQueryBuilderRoute> TheSumOf()
        {
            return new MemoryQueryBuilderOptimizerAggregate(Reference, OptimizerAggregateMode.Sum);
        }
    }
}