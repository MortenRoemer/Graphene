using System;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class OptimizerAggregate : IOptimizerAggregate<BuilderRoute>
    {
        internal OptimizerAggregate(BuilderRoute reference, OptimizerAggregateMode aggregateMode)
        {
            Reference = reference ?? throw new ArgumentNullException(nameof(reference));
            AggregateMode = aggregateMode;
        }

        private OptimizerAggregateMode AggregateMode { get; }

        private BuilderRoute Reference { get; }

        public IOptimizerAttribute<BuilderRoute> Attribute(string name)
        {
            return new OptimizerAttribute(Reference, AggregateMode, name);
        }
    }
}