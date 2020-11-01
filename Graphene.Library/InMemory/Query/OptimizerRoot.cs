using System;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class OptimizerRoot : IQueryBuilderOptimizer<IQueryBuilderRoute>
    {
        internal OptimizerRoot(BuilderRoute reference)
        {
            Reference = reference ?? throw new ArgumentNullException(nameof(reference));
        }

        private BuilderRoute Reference { get; }

        public IOptimizerAggregate<IQueryBuilderRoute> TheAverageOf()
        {
            return new OptimizerAggregate(Reference, OptimizerAggregateMode.Average);
        }

        public IOptimizerAggregate<IQueryBuilderRoute> TheMaximumOf()
        {
            return new OptimizerAggregate(Reference, OptimizerAggregateMode.Maximum);
        }

        public IOptimizerAggregate<IQueryBuilderRoute> TheMinimumOf()
        {
            return new OptimizerAggregate(Reference, OptimizerAggregateMode.Minimum);
        }

        public IOptimizerAggregate<IQueryBuilderRoute> TheSumOf()
        {
            return new OptimizerAggregate(Reference, OptimizerAggregateMode.Sum);
        }
    }
}