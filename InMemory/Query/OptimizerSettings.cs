namespace Graphene.InMemory.Query
{
    internal class OptimizerSettings
    {
        public OptimizerSettings(OptimizerAggregateMode aggregateMode, string attributeName, OptimizerTargetMode targetMode)
        {
            AggregateMode = aggregateMode;
            AttributeName = attributeName;
            TargetMode = targetMode;
        }

        public OptimizerAggregateMode AggregateMode { get; }

        public string AttributeName { get; }

        public OptimizerTargetMode TargetMode { get; }
    }
}