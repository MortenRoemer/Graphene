namespace Graphene.InMemory.Query
{
    internal enum OptimizerAggregateMode : byte
    {
        Sum = 0,
        Average = 1,
        Minimum = 2,
        Maximum = 3,
    }
}