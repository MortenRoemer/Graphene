using System;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class OptimizerRoot : IQueryBuilderOptimizer<BuilderRoute>
    {
        internal OptimizerRoot(BuilderRoute reference)
        {
            Reference = reference ?? throw new ArgumentNullException(nameof(reference));
        }

        private BuilderRoute Reference { get; }

        public IOptimizerAggregate<BuilderRoute> TheAverageOf()
        {
            return new OptimizerAggregate(Reference, OptimizerAggregateMode.Average);
        }

        public IOptimizerAggregate<BuilderRoute> TheMaximumOf()
        {
            return new OptimizerAggregate(Reference, OptimizerAggregateMode.Maximum);
        }

        public IOptimizerAggregate<BuilderRoute> TheMinimumOf()
        {
            return new OptimizerAggregate(Reference, OptimizerAggregateMode.Minimum);
        }

        public IOptimizerAggregate<BuilderRoute> TheSumOf()
        {
            return new OptimizerAggregate(Reference, OptimizerAggregateMode.Sum);
        }
    }
}