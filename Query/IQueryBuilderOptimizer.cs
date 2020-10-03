namespace Graphene.Query
{
    public interface IQueryBuilderOptimizer<T>
    {
        IOptimizerAggregate<T> TheAverageOf();

        IOptimizerAggregate<T> TheMaximumOf();

        IOptimizerAggregate<T> TheMinimumOf();

        IOptimizerAggregate<T> TheSumOf();
    }
}