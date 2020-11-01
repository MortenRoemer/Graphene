using System;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class OptimizerAttribute : IOptimizerAttribute<IQueryBuilderRoute>
    {
        internal OptimizerAttribute(BuilderRoute reference, OptimizerAggregateMode aggregateMode, string attributeName)
        {
            Reference = reference ?? throw new ArgumentNullException(nameof(reference));
            AggregateMode = aggregateMode;
            AttributeName = attributeName ?? throw new ArgumentNullException(nameof(attributeName));
        }

        private OptimizerAggregateMode AggregateMode { get; }

        private string AttributeName { get; }

        private BuilderRoute Reference { get; }

        public IQueryBuilderRoute IsMaximal()
        {
            Reference.SetOptimizerSettings(new OptimizerSettings(AggregateMode, AttributeName, OptimizerTargetMode.Maximum));
            return Reference;
        }

        public IQueryBuilderRoute IsMinimal()
        {
            Reference.SetOptimizerSettings(new OptimizerSettings(AggregateMode, AttributeName, OptimizerTargetMode.Minimum));
            return Reference;
        }
    }
}